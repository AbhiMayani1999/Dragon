import { Component } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { FormFieldConfig } from '../dycrudfrom.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-numberfield',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './numberfield.component.html',
  styleUrl: './numberfield.component.scss'
})
export class NumberfieldComponent {
  public config: FormFieldConfig;
  public isSubmitted: boolean;
  public formGroup: FormGroup;
}
