import { Component } from '@angular/core';
import { FormFieldConfig } from '../dycrudfrom.model';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-passwordfield',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './passwordfield.component.html',
  styleUrl: './passwordfield.component.scss'
})
export class PasswordfieldComponent {
  public config: FormFieldConfig;
  public isSubmitted: boolean;
  public formGroup: FormGroup;
}
