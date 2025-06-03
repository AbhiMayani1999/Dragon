import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output, forwardRef } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { StatusFlags, eMessageType } from '@core/models/data.model';
import { DataService } from '@core/services/data.service';
import { OptionService } from '@core/services/option.service';
import { NgbAccordionModule, NgbNavModule } from '@ng-bootstrap/ng-bootstrap';
import { Modules } from '@urls';
import { Subscription } from 'rxjs';
import { DycrudtableComponent } from '../dycrudtable/dycrudtable.component';
import { FormfieldNames, FromFieldDirective, ValidatorNames } from './dycrudform.directive';
import { FormFieldConfig, FormFields, FromConfig } from './dycrudfrom.model';
import { TableConfig } from '../dycrudtable/dycrudtable.model';

@Component({
  selector: 'app-dycrudform',
  standalone: true,
  templateUrl: './dycrudform.component.html',
  styleUrl: './dycrudform.component.scss',
  imports: [CommonModule, ReactiveFormsModule, FromFieldDirective, NgbAccordionModule, NgbNavModule, forwardRef(() => DycrudtableComponent)]
})
export class DycrudformComponent {
  public formConfig: FromConfig;
  public subTables: TableConfig[];
  public fieldConfig: FormFieldConfig[];

  public dynamicForm: FormGroup;
  public isSubmitForm: boolean = false;
  public isExpandedMode: boolean = false;
  private dynaOptionSubscription: Subscription;

  @Input() set config(data: FromConfig) {
    if (data) {
      this.subTables = [];
      this.fieldConfig = [];
      this.formConfig = { ...data };
      data.fields.filter(d => Object.keys(d)[0] === FormfieldNames.Table).forEach(d => this.subTables.push(d[Object.keys(d)[0]]));
      this.formConfig.fields = data.fields.filter(d => Object.keys(d)[0] !== FormfieldNames.Table);
      this.onGenerateForm();
    }
  }

  @Output() onClose: EventEmitter<boolean> = new EventEmitter<boolean>();

  constructor(public formBuilder: FormBuilder, public dataService: DataService, public optionService: OptionService) { }

  private onGenerateForm(): void {
    this.dynamicForm = this.createFormControl(this.formConfig.data);
  }

  private createFormControl(data: any): FormGroup {
    const group: FormGroup = this.formBuilder.group({});
    group.addControl(FormFields.Id, new FormControl(0));
    this.formConfig.fields.forEach((field) => {
      const fieldType = Object.keys(field)[0];
      const fieldConfig: FormFieldConfig = field[fieldType];
      let composedValidations: ValidatorFn;
      if (fieldConfig) {
        if (fieldConfig.validation && fieldConfig.validation.length) {
          const validList = [];
          fieldConfig.validation.forEach((validation) => {
            switch (Object.keys(validation)[0]) {
              case ValidatorNames.RequiredValidation: {
                validList.push(Validators.required);
                validation.type = 'required'; validation.message = validation.message ? validation.message : `${fieldConfig.title} is required`;
                break;
              }
              case ValidatorNames.EmailValidation: {
                validList.push(Validators.email);
                validation.type = 'email'; validation.message = validation.message ? validation.message : `Enter valid email address`;
                break;
              }
              default: { break; }
            }
          });
          composedValidations = Validators.compose(validList);
        }

        if (fieldConfig.optionTransfer) { this.optionService.pushTransfer(fieldConfig.optionTransfer); }
        let defaultValue: any;
        if (fieldType === FormfieldNames.CheckboxField || fieldType === FormfieldNames.SwitchField) { defaultValue = fieldConfig.checked ? true : false; } else { defaultValue = ""; }
        if (!fieldConfig.identifier) { this.fieldConfig.push(field); } else { if (!data) { data = {}; } data[fieldConfig.jsonProperty] = this.formConfig.parentIdentifier; }

        group.addControl(fieldConfig.jsonProperty, new FormControl(defaultValue, composedValidations));
      }
    });
    this.getOption();
    if (data) { group.patchValue(data); this.subTables.forEach(table => table.parentIdentifier = data.id); }
    return group;
  }

  private getOption() {
    if (!this.dynaOptionSubscription) {
      this.dynaOptionSubscription = this.optionService.optionSubject.subscribe(data => {
        if (data) {
          this.fieldConfig.forEach(item => {
            const field: FormFieldConfig = item[Object.keys(item)[0]];
            if (field.optionTransfer) {
              const optionTransfer = this.optionService.optionTransferList.find(o =>
                (o.table === field.optionTransfer.table && o.key === field.optionTransfer.key && o.value === field.optionTransfer.value) ||
                o.keystore === field.optionTransfer.keystore);
              if (optionTransfer) { item[Object.keys(item)[0]].options = optionTransfer.options; }
            }
          });
        }
      });
    }
    this.optionService.processTransfer();
  }

  public submitForm() {
    this.isSubmitForm = true;
    if (this.dynamicForm.valid) {
      this.dataService.post(`${Modules.Base}${this.formConfig.submiturl}`, this.dynamicForm.value).then((response: any) => {
        if (response.status === StatusFlags.Success) {
          if (this.subTables && this.subTables.length && response.data && response.data.id) {
            this.isSubmitForm = false;
            this.formConfig.data = response.data;
            this.subTables.forEach(table => table.parentIdentifier = response.data.id);
            this.resetForm();
            return;
          }
          if (this.subTables && this.subTables.length && !response.data) { this.dataService.notify.next({ key: eMessageType.Info, value: 'Missing parent data ...!' }); }
          this.dynamicForm.reset(); this.onClose.emit(true);
        }
      });
    }
  }

  public resetForm() {
    this.dynamicForm.reset(); this.dynamicForm.patchValue(this.formConfig.data);
  }

  public closeForm() {
    this.onClose.emit(false);
  }
}

// let patchData = {};
// const parentFields = data.fields.filter((d) => d.notInSubForm === true);
// parentFields.forEach((field) => { patchData = { ...patchData, [field.name]: formInput.parentData.data }; });
// formInput.formData = { ...formInput.formData, ...patchData };
