import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { RouterModule, Routes } from '@angular/router';
import { NgxSpinnerModule } from 'ngx-spinner';
import { AppComponent } from './app.component';

const routes: Routes = [
  { path: '', loadChildren: () => import('./root.module').then(m => m.RootModule) },
  { path: '**', loadChildren: () => import('./root.module').then(m => m.RootModule) },
];

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    NgxSpinnerModule.forRoot({ type: 'ball-clip-rotate' }),
    RouterModule.forRoot(routes, { scrollPositionRestoration: 'top' })
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
