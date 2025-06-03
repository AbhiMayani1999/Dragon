import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';


@Injectable()
export class JwtInterceptor implements HttpInterceptor {
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        if (sessionStorage.AccessToken) {
            request = request.clone({ setHeaders: { Authorization: `Bearer ${sessionStorage.AccessToken}` } });
        }
        return next.handle(request);
    }
}
