import { CommonModule } from '@angular/common';
import { HTTP_INTERCEPTORS, HttpClient, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { RouterModule, Routes } from '@angular/router';
import { StoreModule } from '@ngrx/store';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { AppService } from '@services/app.service';
import { AuthGuard } from '@services/interceptor/auth.guard';
import { indexReducer } from '@store/index.reducer';
import { FixedRoutes } from '@urls';
import { HIGHLIGHT_OPTIONS, HighlightModule } from 'ngx-highlightjs';
import { QuillModule } from 'ngx-quill';
import { NgScrollbarModule } from 'ngx-scrollbar';
import { RootComponent } from './root.component';
import { JwtInterceptor } from '@services/helpers/jwt.interceptor';
import { DataService } from '@services/data.service';
import { NotificationService } from '@services/notification.service';
import { ErrorInterceptor } from '@services/helpers/error.interceptor';
import { NavService } from '@services/nav.service';

export const rootRoutes: Routes = [
  { path: FixedRoutes.Auth, component: RootComponent, loadChildren: () => import('../auth/auth.module').then((m) => m.AuthModule), },
  { path: '**', component: RootComponent, canActivate: [AuthGuard], loadChildren: () => import('../manager/manager.module').then(m => m.ManagerModule) }
];

@NgModule({
  declarations: [RootComponent],
  imports: [
    CommonModule,
    HttpClientModule,
    RouterModule.forChild(rootRoutes),
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: httpTranslateLoader,
        deps: [HttpClient],
      },
    }),
    StoreModule.forRoot({ index: indexReducer }),
    NgScrollbarModule.withConfig({ visibility: 'hover', appearance: 'standard' }),
    HighlightModule,
    QuillModule.forRoot()
  ],
  providers: [
    Title,
    AppService,
    DataService,
    NavService,
    NotificationService,
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    {
      provide: HIGHLIGHT_OPTIONS,
      useValue: {
        coreLibraryLoader: () => import('highlight.js/lib/core'),
        languages: {
          json: () => import('highlight.js/lib/languages/json'),
          typescript: () => import('highlight.js/lib/languages/typescript'),
          xml: () => import('highlight.js/lib/languages/xml'),
        },
      },
    }
  ],
})
export class RootModule { }

// AOT compilation support
export function httpTranslateLoader(http: HttpClient) {
  return new TranslateHttpLoader(http);
}
