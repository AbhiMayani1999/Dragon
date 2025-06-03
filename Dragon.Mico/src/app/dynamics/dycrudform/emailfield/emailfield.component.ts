import { Component } from '@angular/core';
import { FormFieldConfig } from '../dycrudfrom.model';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-emailfield',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './emailfield.component.html',
  styleUrl: './emailfield.component.scss'
})
export class EmailfieldComponent {
  public config: FormFieldConfig;
  public isSubmitted: boolean;
  public formGroup: FormGroup;
}
