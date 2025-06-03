import { Injectable, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import Swal from 'sweetalert2';
import { DataService } from './data.service';

@Injectable({ providedIn: 'root' })
export class NotificationService implements OnDestroy {
  notificationSubscription: Subscription;
  toast = Swal.mixin({
    toast: true,
    timer: 3000,
    position: 'bottom-end',
    showConfirmButton: false,
    customClass: 'sweet-alerts',
  });
  constructor(private dataService: DataService) { }

  listenToNotify(): void {
    if (!this.notificationSubscription) {
      this.notificationSubscription = this.dataService.notify.subscribe((data) => this.notify(data.key, data.value));
    }
  }

  notify(status: any, message: any): void {
    if (message) {
      if (message.status === 0) {
        message = 'Internet connection failed';
      } else if (message.status === 400) {
        message = message.error.message;
      }
      this.toast.fire({ icon: status.toLowerCase(), title: message });
    }
  }

  ngOnDestroy(): void {
    if (this.notificationSubscription) {
      this.notificationSubscription.unsubscribe();
    }
  }
}
