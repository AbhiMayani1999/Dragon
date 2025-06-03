import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule, Routes } from '@angular/router';
import { AppService } from '@services/app.service';
import { AppComponent } from './app.component';
import {  HttpClientModule } from '@angular/common/http';

export const appRoutes: Routes = [
  {
    path: '',
    children: [
      { path: '', loadChildren: () => import('./root/root.module').then((m) => m.RootModule) },
      { path: '**', loadChildren: () => import('./root/root.module').then((m) => m.RootModule) },
    ],
  },
];

@NgModule({
  imports: [
    RouterModule.forRoot(appRoutes, { scrollPositionRestoration: 'enabled' }),
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule
  ],
  declarations: [AppComponent],
  bootstrap: [AppComponent],
  providers: [AppService]
})
export class AppModule { }
