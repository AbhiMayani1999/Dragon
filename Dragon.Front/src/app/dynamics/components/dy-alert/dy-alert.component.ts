import { Component, Input, Output, ViewChild, EventEmitter } from '@angular/core';
import { ModalComponent } from 'angular-custom-modal';
import { AlertConfig } from '../../models/dy-alert.model';

@Component({
  selector: 'app-dy-alert',
  templateUrl: './dy-alert.component.html',
  styleUrls: ['./dy-alert.component.css']
})
export class DyAlertComponent {
  @Input() config: AlertConfig;
  @Output() onConfirm: EventEmitter<boolean> = new EventEmitter<boolean>();
  @ViewChild('template', { static: true }) modelPropertyReference: ModalComponent;

  public openModel(): void {
    this.modelPropertyReference.open();
  }

  public onClosePopup(): void {
    this.modelPropertyReference.close();
  }

  public onConfirmClick(): void {
    this.onConfirm.emit(true);
    this.onClosePopup();
  }
}
