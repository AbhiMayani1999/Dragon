import { Directive, Input, ViewContainerRef } from '@angular/core';
import { TextfieldComponent } from './textfield/textfield.component';
import { NumberfieldComponent } from './numberfield/numberfield.component';
import { SelectfieldComponent } from './selectfield/selectfield.component';
import { EmailfieldComponent } from './emailfield/emailfield.component';
import { DatefieldComponent } from './datefield/datefield.component';
import { CheckboxfieldComponent } from './checkboxfield/checkboxfield.component';
import { FormFieldConfig } from './dycrudfrom.model';
import { FormGroup } from '@angular/forms';
import { SwitchfieldComponent } from './switchfield/switchfield.component';
import { ImageuploadfieldComponent } from './uploadfield/imageuploadfield/imageuploadfield.component';
import { FileuploadfieldComponent } from './uploadfield/fileuploadfield/fileuploadfield.component';
import { PasswordfieldComponent } from './passwordfield/passwordfield.component';

@Directive({ selector: 'formfield', standalone: true })
export class FromFieldDirective {
  componentRef: any;
  constructor(private container: ViewContainerRef) { }
  @Input() set config(data: AwFromFieldInput) {
    const componentType = Object.keys(data.field)[0];
    const widget = FromfieldMapper[componentType];
    if (widget) {
      if (!this.componentRef) {
        this.componentRef = this.container.createComponent(widget);
      }
      this.componentRef.instance.config = data.field[componentType];
      this.componentRef.instance.formGroup = data.formGroup;
      this.componentRef.instance.isSubmitted = data.isSubmitted;
    } else {
      console.warn(`Form Component type not found: ${componentType}`);
    }
  }
}

export interface AwFromFieldInput {
  field: FormFieldConfig;
  formGroup: FormGroup;
  isSubmitted: boolean;
}

export enum ValidatorNames {
  RequiredValidation = 'requiredValidation',
  EmailValidation = 'emailValidation',
}

export enum FormfieldNames {
  TextField = 'textField',
  NumberField = 'numberField',
  SelectField = 'selectField',
  MultiselectField = 'multiselectField',
  EmailField = 'emailField',
  DateField = 'dateField',
  SwitchField = 'switchField',
  CheckboxField = 'checkboxField',
  ImageuploadField = 'imageuploadField',
  FileuploadField = 'fileuploadField',
  PasswordField = 'passwordField',
  Table = 'table'
}

export const FromfieldMapper = {
  [FormfieldNames.TextField]: TextfieldComponent,
  [FormfieldNames.PasswordField]: PasswordfieldComponent,
  [FormfieldNames.NumberField]: NumberfieldComponent,
  [FormfieldNames.SelectField]: SelectfieldComponent,
  [FormfieldNames.EmailField]: EmailfieldComponent,
  [FormfieldNames.DateField]: DatefieldComponent,
  [FormfieldNames.SwitchField]: SwitchfieldComponent,
  [FormfieldNames.CheckboxField]: CheckboxfieldComponent,
  [FormfieldNames.ImageuploadField]: ImageuploadfieldComponent,
  [FormfieldNames.FileuploadField]: FileuploadfieldComponent
};
