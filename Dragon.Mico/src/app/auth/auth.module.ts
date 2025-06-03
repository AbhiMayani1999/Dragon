import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbCarouselModule } from '@ng-bootstrap/ng-bootstrap';
import { NgOtpInputModule } from 'ng-otp-input';
import { CarouselModule } from 'ngx-owl-carousel-o';

import { RouterModule, Routes } from '@angular/router';
import { ConfirmmailComponent } from './confirmmail/confirmmail.component';
import { LockscreenComponent } from './lockscreen/lockscreen.component';
import { LoginComponent } from './login/login.component';
import { LogoutComponent } from './logout/logout.component';
import { RecoverpwdComponent } from './recoverpwd/recoverpwd.component';
import { RegisterComponent } from './register/register.component';
import { TwostepverificationComponent } from './twostepverification/twostepverification.component';
import { VerificationComponent } from './verification/verification.component';

const routes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'recoverpw', component: RecoverpwdComponent },
  { path: 'lock-screen', component: LockscreenComponent },
  { path: 'logout', component: LogoutComponent },
  { path: 'confirm-mail', component: ConfirmmailComponent },
  { path: 'email-verification', component: VerificationComponent },
  { path: 'two-step-verification', component: TwostepverificationComponent }
];

@NgModule({
  declarations: [
    LoginComponent,
    RegisterComponent,
    RecoverpwdComponent,
    LockscreenComponent,
    LogoutComponent,
    ConfirmmailComponent,
    VerificationComponent,
    TwostepverificationComponent
  ],
  imports: [
    CommonModule,
    CarouselModule,
    ReactiveFormsModule,
    FormsModule,
    NgOtpInputModule,
    NgbCarouselModule,
    RouterModule.forChild(routes)
  ]
})
export class AuthModule { }
