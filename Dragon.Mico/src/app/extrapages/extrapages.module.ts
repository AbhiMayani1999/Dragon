import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { NgbCarouselModule } from '@ng-bootstrap/ng-bootstrap';
import { CarouselModule } from 'ngx-owl-carousel-o';

import { RouterModule, Routes } from '@angular/router';
import { ComingsoonComponent } from './comingsoon/comingsoon.component';
import { MaintenanceComponent } from './maintenance/maintenance.component';
import { Page404Component } from './page404/page404.component';
import { Page500Component } from './page500/page500.component';

const routes: Routes = [
  { path: 'maintenance', component: MaintenanceComponent },
  { path: 'coming-soon', component: ComingsoonComponent },
  { path: '404', component: Page404Component },
  { path: '500', component: Page500Component }
];

@NgModule({
  declarations: [
    MaintenanceComponent,
    ComingsoonComponent,
    Page404Component,
    Page500Component
  ],
  imports: [
    CommonModule,
    CarouselModule,
    NgbCarouselModule,
    RouterModule.forChild(routes)
  ]
})
export class ExtrapagesModule { }
