import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FixedRoutes } from '@urls';
import { MenuModule } from 'headlessui-angular';
import { AuthComponent } from './auth.component';
import { IconModule } from '../core/icon/icon.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

const routes: Routes = [
  { path: '', component: AuthComponent },
  { path: FixedRoutes.Auth, component: AuthComponent }
];

@NgModule({
  declarations: [
    AuthComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forChild(routes),
    MenuModule,
    IconModule
  ]
})
export class AuthModule { }
