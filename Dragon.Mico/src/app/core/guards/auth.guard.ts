import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, createUrlTreeFromSnapshot, } from '@angular/router';
import { map } from 'rxjs';
import { FixedRoutes } from 'src/app/app.urls';
import { AuthService } from '../services/auth.service';

export const AuthGuard = (next: ActivatedRouteSnapshot) => {
    return inject(AuthService).isAuthenticated.pipe(map((isLoggedIn: boolean) => isLoggedIn ? true : createUrlTreeFromSnapshot(next, ['/', FixedRoutes.Auth])));
};