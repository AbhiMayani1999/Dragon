import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LayoutComponent } from '../layouts/layout.component';
import { LayoutsModule } from '../layouts/layouts.module';
import { DynamicsComponent } from './dynamics.component';
import { FlatpickrModule } from 'angularx-flatpickr';

const routes: Routes = [
  {
    path: '', component: LayoutComponent,
    children: [
      { path: '', component: DynamicsComponent },
      { path: '**', component: DynamicsComponent }
    ]
  }
];

@NgModule({
  imports: [
    CommonModule,
    LayoutsModule,
    HttpClientModule,
    FlatpickrModule.forRoot(),
    RouterModule.forChild(routes)
  ],
})
export class DynamicsModule { }
