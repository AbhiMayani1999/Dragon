import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { FixedRoutes } from 'src/app/app.urls';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(catchError(err => {
      if (err.status === 401) {
        err.message = "Unauthorised";
        sessionStorage.clear();
        this.router.navigateByUrl(FixedRoutes.Auth);
      } else if (err.status === 404) {
        err.message = "Not Found";
      }
      return throwError(() => err);
    }))
  }
}
