import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CoreModule } from '../core/core.module';
import { ManagerComponent } from './manager.component';
import { DynamicsModule } from '../dynamics/dynamics.module';

export const managerRoutes: Routes = [
  { path: '**', component: ManagerComponent }
];

@NgModule({
  declarations: [
    ManagerComponent,
  ],
  imports: [
    CommonModule,
    CoreModule,
    DynamicsModule,
    RouterModule.forChild(managerRoutes)
  ]
})
export class ManagerModule { }
