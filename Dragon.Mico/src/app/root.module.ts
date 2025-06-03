import { CommonModule } from '@angular/common';
import { HTTP_INTERCEPTORS, HttpClient, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NotifyService } from '@core/services/notify.service';
import { OptionService } from '@core/services/option.service';
import { PageService } from '@core/services/page.service';
import { NgbModule, NgbNavModule, NgbPopoverModule, NgbTooltipModule } from '@ng-bootstrap/ng-bootstrap';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { FixedRoutes } from './app.urls';
import { AuthGuard } from './core/guards/auth.guard';
import { ErrorInterceptor } from './core/helpers/error.interceptor';
import { JwtInterceptor } from './core/helpers/jwt.interceptor';
import { AuthService } from './core/services/auth.service';
import { DataService } from './core/services/data.service';
import { NavService } from './core/services/nav.service';
import { LoadingInterceptor } from '@core/guards/loading.interceptor.guard';

const routes: Routes = [
  { path: FixedRoutes.Auth, loadChildren: () => import('./auth/auth.module').then(m => m.AuthModule) },
  { path: '', loadChildren: () => import('./dynamics/dynamics.module').then(m => m.DynamicsModule), canActivate: [AuthGuard] },
  { path: '**', loadChildren: () => import('./dynamics/dynamics.module').then(m => m.DynamicsModule), canActivate: [AuthGuard] },
];

export function createTranslateLoader(http: HttpClient): any {
  return new TranslateHttpLoader(http, 'assets/i18n/', '.json');
}

@NgModule({
  declarations: [],
  imports: [
    NgbModule,
    CommonModule,
    NgbNavModule,
    NgbTooltipModule,
    NgbPopoverModule,
    HttpClientModule,
    RouterModule.forChild(routes),
    TranslateModule.forRoot({ defaultLanguage: 'en', loader: { provide: TranslateLoader, useFactory: (createTranslateLoader), deps: [HttpClient] } }),
  ],
  providers: [
    NavService,
    AuthService,
    DataService,
    PageService,
    OptionService,
    NotifyService,
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true }
  ],
})
export class RootModule { }
