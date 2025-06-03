import { CheckboxfieldComponent } from "./components/checkboxfield/checkboxfield.component";
import { DatefieldComponent } from "./components/datefield/datefield.component";
import { EmailfieldComponent } from "./components/emailfield/emailfield.component";
import { NumberfieldComponent } from "./components/numberfield/numberfield.component";
import { SelectfieldComponent } from "./components/selectfield/selectfield.component";
import { TextfieldComponent } from "./components/textfield/textfield.component";

export enum ValidatorNames {
    RequiredValidation = 'requiredValidation',
    EmailValidation = 'emailValidation',
}

export enum FormfieldNames {
    TextField = 'textField',
    NumberField = 'numberField',
    SelectField = 'selectField',
    EmailField = 'emailField',
    DateField = 'dateField',
    CheckboxField = 'checkboxField'
}

export const FromfieldMapper = {
    [FormfieldNames.TextField]: TextfieldComponent,
    [FormfieldNames.NumberField]: NumberfieldComponent,
    [FormfieldNames.SelectField]: SelectfieldComponent,
    [FormfieldNames.EmailField]: EmailfieldComponent,
    [FormfieldNames.DateField]: DatefieldComponent,
    [FormfieldNames.CheckboxField]: CheckboxfieldComponent,
};
