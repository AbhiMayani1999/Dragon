import { Component } from '@angular/core';
import { FormFieldConfig } from '../../awesomefrom.model';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-checkboxfield',
  templateUrl: './checkboxfield.component.html',
  styleUrls: ['./checkboxfield.component.css']
})
export class CheckboxfieldComponent {
  public config: FormFieldConfig;
  public isSubmitted: boolean;
  public formGroup: FormGroup;
}
