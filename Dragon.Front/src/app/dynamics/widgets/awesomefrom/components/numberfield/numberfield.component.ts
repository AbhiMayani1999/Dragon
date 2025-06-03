import { Component } from '@angular/core';
import { FormFieldConfig } from '../../awesomefrom.model';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-numberfield',
  templateUrl: './numberfield.component.html',
  styleUrls: ['./numberfield.component.css']
})
export class NumberfieldComponent {
  public config: FormFieldConfig;
  public isSubmitted: boolean;
  public formGroup: FormGroup;
}
