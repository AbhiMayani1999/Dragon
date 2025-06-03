import { Component } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { FormFieldConfig } from '../../awesomefrom.model';

@Component({
  selector: 'app-textfield',
  templateUrl: './textfield.component.html',
  styleUrls: ['./textfield.component.css']
})
export class TextfieldComponent {
  public config: FormFieldConfig;
  public isSubmitted: boolean;
  public formGroup: FormGroup;
}
