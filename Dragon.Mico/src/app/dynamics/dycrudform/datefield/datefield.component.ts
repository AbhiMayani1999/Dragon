import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { FlatpickrModule } from 'angularx-flatpickr';
import { FormFieldConfig } from '../dycrudfrom.model';

@Component({
  selector: 'app-datefield',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FlatpickrModule],
  templateUrl: './datefield.component.html',
  styleUrl: './datefield.component.scss'
})
export class DatefieldComponent {
  public config: FormFieldConfig;
  public isSubmitted: boolean;
  public formGroup: FormGroup;
}
