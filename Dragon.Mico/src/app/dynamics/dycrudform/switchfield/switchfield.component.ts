import { Component } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { UiSwitchModule } from 'ngx-ui-switch';
import { FormFieldConfig } from '../dycrudfrom.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-switchfield',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, UiSwitchModule],
  templateUrl: './switchfield.component.html',
  styleUrl: './switchfield.component.scss'
})
export class SwitchfieldComponent {
  public config: FormFieldConfig;
  public isSubmitted: boolean;
  public formGroup: FormGroup;
}
