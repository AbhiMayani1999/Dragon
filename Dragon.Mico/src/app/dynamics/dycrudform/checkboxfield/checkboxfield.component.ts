import { Component } from '@angular/core';
import { FormFieldConfig } from '../dycrudfrom.model';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-checkboxfield',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './checkboxfield.component.html',
  styleUrl: './checkboxfield.component.scss'
})
export class CheckboxfieldComponent {
  public config: FormFieldConfig;
  public isSubmitted: boolean;
  public formGroup: FormGroup;
}
