import { Component } from '@angular/core';
import { FormFieldConfig } from '../../awesomefrom.model';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-emailfield',
  templateUrl: './emailfield.component.html',
  styleUrls: ['./emailfield.component.css']
})
export class EmailfieldComponent {
  public config: FormFieldConfig;
  public isSubmitted: boolean;
  public formGroup: FormGroup;
}
