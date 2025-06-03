import { animate, style, transition, trigger } from '@angular/animations';
import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { AppService } from '@services/app.service';
import { AuthService } from '@services/auth.service';
import { FixedRoutes } from '@urls';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.scss'],
  animations: [
    trigger('toggleAnimation', [
      transition(':enter', [style({ opacity: 0, transform: 'scale(0.95)' }), animate('100ms ease-out', style({ opacity: 1, transform: 'scale(1)' }))]),
      transition(':leave', [animate('75ms', style({ opacity: 0, transform: 'scale(0.95)' }))]),
    ]),
  ],
})
export class AuthComponent {
  readonly FixedRoutes = FixedRoutes;
  store: any;
  changeLanguage(item: any) {
    this.translate.use(item.code);
    this.appSetting.toggleLanguage(item);
    this.storeData.dispatch({ type: 'toggleRTL', payload: this.store.locale?.toLowerCase() === 'ae' ? 'rtl' : 'ltr' });
    window.location.reload();
  }

  public loginForm: FormGroup = new FormGroup({
    username: new FormControl('', Validators.required),
    password: new FormControl('', Validators.required),
  });

  constructor(public translate: TranslateService, public storeData: Store<any>, private appSetting: AppService, private authService: AuthService) {
    this.initStore();
  }

  async initStore() {
    this.storeData.select((d) => d.index).subscribe((d) => (this.store = d));
  }

  onSubmit() {
    if (this.loginForm.valid) {
      this.authService.login(this.loginForm.value);
    }
  }
}
