import { Component } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { FormFieldConfig } from '../dycrudfrom.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-textfield',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './textfield.component.html',
  styleUrl: './textfield.component.scss'
})
export class TextfieldComponent {
  public config: FormFieldConfig;
  public isSubmitted: boolean;
  public formGroup: FormGroup;
}
