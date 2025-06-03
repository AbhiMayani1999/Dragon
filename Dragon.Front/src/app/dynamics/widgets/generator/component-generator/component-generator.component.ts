import { Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DataService } from '@services/data.service';
import { ComponentStructure } from '@services/model/dynamics/generator.model';
import { Modules } from '@urls';
import { ModalComponent } from 'angular-custom-modal';

@Component({
  selector: 'app-component-generator',
  templateUrl: './component-generator.component.html',
  styleUrls: ['./component-generator.component.css'],
})
export class ComponentGeneratorComponent {
  @Input() parentComponents!: ComponentStructure[];
  @Output() onClose: EventEmitter<boolean> = new EventEmitter<boolean>();

  public generatorForm!: FormGroup;
  public selectedGroup: Array<string> = [];
  public allComponent: Array<any> = [];
  public allComponentName: Array<string> = [];
  public dyComponentList: any;
  public genericProperty: Array<any> = [];
  public selectedProperty: Array<any> = [];
  public selectedPropertyIds: Array<number> = [];
  public selectedPropertyMultiple: Array<number> = [];
  public customProperty: Array<any> = [];
  public customProp: string = '';

  @ViewChild('template', { static: true }) modelComponentReference: ModalComponent;

  constructor(private dataService: DataService) {
    this.onGenerateNewForm();
  }

  public onGenerateNewComponent() {
    this.resetData();
    this.onGenerateNewForm();
    this.getAllProperties();
    this.modelComponentReference.open();
  }

  private onGenerateNewForm() {
    this.generatorForm = new FormGroup({
      id: new FormControl(0),
      name: new FormControl('', Validators.required),
      group: new FormControl(),
      properties: new FormControl(),
      canBeParent: new FormControl(false),
      propertyList: new FormControl(''),
      parentMapping: new FormControl(),
    });
  }

  onUpdateComponent(id: number) {
    this.resetData();
    this.getDataById(id);
    this.getAllProperties();
    this.modelComponentReference.open();
  }

  public onSelectGroup(data: Array<any>): void {
    const groups = this.allComponent.filter((d: any) => data.includes(d.name)).map((d) => d.id);
    this.generatorForm.get('parentMapping')?.setValue(groups);
  }

  public getDataById(id: number = 0): void {
    this.dataService.getData(`${Modules.ComponentStructure}/${id}`).then((res: any) => {
      if (res) {
        this.dyComponentList = res[0];
        if (id == 0) {
          this.allComponent = this.dyComponentList;
          this.allComponentName = this.dyComponentList.map((d: any) => d.name);
        } else {
          this.selectedProperty = [];
          this.selectedGroup = this.dyComponentList.parentMapping;
          this.selectedProperty = this.dyComponentList.propertyList;
          this.selectedPropertyIds = this.selectedProperty.map((d) => d.key);
          this.selectedPropertyMultiple = this.selectedProperty.filter((d) => d.value).map((d) => d.key);
          this.generatorForm.patchValue(this.dyComponentList);
        }
      }
    });
  }

  public onSelectMultiple(event: any, property: any): void {
    const properties = this.generatorForm.get('properties').value;
    const index = properties.findIndex((d) => d.componentPropertyId == property.id);
    if (index > -1) {
      properties[index].isMultiple = event.currentTarget.checked;
    }
    this.generatorForm.get('properties').setValue(properties);
    this.selectedPropertyMultiple = properties.filter((d) => d.isMultiple).map((d) => d.componentPropertyId);
  }

  public onSelectProperty(event: any, property: any): void {
    const properties = this.generatorForm.get('properties').value ?? [];
    if (event.currentTarget.checked) {
      properties.push({ id: 0, componentPropertyId: property.id, isMultiple: false });
      this.selectedProperty.push({ key: property.id, value: false });
    } else {
      const propertyIndex = properties.findIndex((d) => d.componentPropertyId == property.id);
      if (propertyIndex > -1) {
        properties.splice(propertyIndex, 1);
      }
    }
    this.generatorForm.get('properties').setValue(properties);
    this.selectedPropertyIds = properties.map((d: any) => d.componentPropertyId);
    this.selectedPropertyMultiple = properties.filter((d) => d.isMultiple).map((d) => d.componentPropertyId);
  }

  public onAddCustomProperty(): void {
    this.dataService.post(`${Modules.ComponentProperty}`, { id: 0, name: this.customProp, isGeneric: false }).then((response: any) => {
      if (response) {
        this.customProp = '';
        this.getAllProperties();
      }
    });
  }

  public getAllProperties(): void {
    this.genericProperty = [];
    this.customProperty = [];
    this.dataService.getData<[]>(`${Modules.ComponentProperty}`).then((res: []) => {
      if (res && res.length) {
        this.genericProperty = res.filter((d: any) => d.isGeneric);
        this.customProperty = res.filter((d: any) => !d.isGeneric);
      }
    });
  }

  public resetData(): void {
    this.generatorForm.reset();
    this.selectedProperty = [];
    this.selectedPropertyIds = [];
    this.selectedPropertyMultiple = [];
    this.selectedGroup = [];
  }

  public onClosePopup(refresh: boolean = false): void {
    this.resetData();
    this.onClose.emit(refresh);
    this.modelComponentReference.close();
  }

  public saveForm(): void {
    if (this.generatorForm.valid) {
      const data = this.generatorForm.value;
      data.parentMapping = this.selectedGroup;
      this.dataService.post(`${Modules.ComponentStructure}`, data).then((res: any) => {
        if (res) {
          this.onClosePopup(true);
        }
      });
    }
  }
}
