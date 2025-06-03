import { Component } from '@angular/core';
import { FormFieldConfig } from '../../awesomefrom.model';
import { FormGroup } from '@angular/forms';
import { FlatpickrOptions } from 'ng2-flatpickr';

@Component({
  selector: 'app-datefield',
  templateUrl: './datefield.component.html',
  styleUrls: ['./datefield.component.css']
})
export class DatefieldComponent {
  public config: FormFieldConfig;
  public isSubmitted: boolean;
  public formGroup: FormGroup;

  public dateFormat = 'd-m-Y';
}
