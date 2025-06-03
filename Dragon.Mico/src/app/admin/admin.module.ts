import { CommonModule } from '@angular/common';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { NgbDropdownModule, NgbNavModule } from '@ng-bootstrap/ng-bootstrap';
import { FeatherModule } from 'angular-feather';
import { allIcons } from 'angular-feather/icons';
import { NgApexchartsModule } from 'ng-apexcharts';
import { CountUpModule } from 'ngx-countup';
import { CarouselModule } from 'ngx-owl-carousel-o';
import { SimplebarAngularModule } from 'simplebar-angular';
import { LeafletModule } from '@asymmetrik/ngx-leaflet';

import { SharedModule } from '../shared/shared.module';
import { WidgetModule } from '../shared/widget/widget.module';
import { AppsModule } from './apps/apps.module';
import { ChartModule } from './chart/chart.module';
import { ComponentsModule } from './components/components.module';
import { ExtendedModule } from './extended/extended.module';
import { ExtraspagesModule } from './extraspages/extraspages.module';
import { FormModule } from './form/form.module';
import { TablesModule } from './tables/tables.module';

import { DashboardComponent } from './dashboard/dashboard.component';
import { LayoutComponent } from '../layouts/layout.component';
import { LayoutsModule } from '../layouts/layouts.module';

const routes: Routes = [
  {
    path: '', component: LayoutComponent,
    children: [
      { path: '', component: DashboardComponent },
      { path: 'apps', loadChildren: () => import('./apps/apps.module').then(m => m.AppsModule) },
      { path: 'pages', loadChildren: () => import('./extraspages/extraspages.module').then(m => m.ExtraspagesModule) },
      { path: 'ui', loadChildren: () => import('./components/components.module').then(m => m.ComponentsModule) },
      { path: 'extended', loadChildren: () => import('./extended/extended.module').then(m => m.ExtendedModule) },
      { path: 'form', loadChildren: () => import('./form/form.module').then(m => m.FormModule) },
      { path: 'tables', loadChildren: () => import('./tables/tables.module').then(m => m.TablesModule) },
      { path: 'chart', loadChildren: () => import('./chart/chart.module').then(m => m.ChartModule) },
      { path: 'icons', loadChildren: () => import('./icons/icons.module').then(m => m.IconsModule) },
      { path: 'maps', loadChildren: () => import('./maps/maps.module').then(m => m.MapsModule) }
    ]
  }
];

@NgModule({
  declarations: [DashboardComponent],
  imports: [
    CommonModule,
    LayoutsModule,
    WidgetModule,
    CountUpModule,
    SharedModule,
    NgApexchartsModule,
    SimplebarAngularModule,
    CarouselModule,
    FeatherModule.pick(allIcons),
    RouterModule,
    NgbDropdownModule,
    NgbNavModule,
    AppsModule,
    ExtraspagesModule,
    ComponentsModule,
    ExtendedModule,
    FormModule,
    TablesModule,
    ChartModule,
    LeafletModule,
    RouterModule.forChild(routes)
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class AdminModule { }
