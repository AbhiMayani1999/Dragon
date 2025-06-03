import { Component, Input } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { ValidatorNames } from './awesomeform.mapper';
import { FormFieldConfig, FromConfig } from './awesomefrom.model';

@Component({
  selector: 'aw-from',
  templateUrl: './awesomefrom.component.html',
  styleUrls: ['./awesomefrom.component.css']
})
export class AwesomefromComponent {
  formConfig: FromConfig;
  dynamicForm: FormGroup;
  @Input() set config(data: FromConfig) {
    this.formConfig = { ...data };
    if (data) {
      // let patchData = {};
      // const parentFields = data.fields.filter((d) => d.notInSubForm === true);
      // parentFields.forEach((field) => { patchData = { ...patchData, [field.name]: formInput.parentData.data }; });
      // formInput.formData = { ...formInput.formData, ...patchData };
      this.onFromGenerate(data);
    }
  }


  private onFromGenerate(data: FromConfig): void {
    this.dynamicForm = this.createFormControl();
  }

  private createFormControl(): FormGroup {
    const group: FormGroup = this.formBuilder.group({});
    this.formConfig.fields.forEach((field) => {
      const fieldConfig: FormFieldConfig = field[Object.keys(field)[0]];
      let composedValidations: ValidatorFn;

      if (fieldConfig.validation && fieldConfig.validation.length > 0) {
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

      group.addControl(fieldConfig.jsonProperty, new FormControl('', composedValidations));
    });
    return group;
  }

  isSubmitForm: boolean = false;

  constructor(public formBuilder: FormBuilder) { }

  submitForm() { this.isSubmitForm = true; }
}

