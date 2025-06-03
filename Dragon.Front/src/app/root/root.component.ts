import { Component } from '@angular/core';
import { NotificationService } from '@services/notification.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-root',
  templateUrl: './root.component.html',
  styleUrls: ['./root.component.scss'],
})
export class RootComponent {
  constructor(private notificationService: NotificationService) { this.notificationService.listenToNotify(); }
}
