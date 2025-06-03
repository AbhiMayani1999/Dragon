import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, createUrlTreeFromSnapshot, } from '@angular/router';
import { AuthService } from '@services/auth.service';
import { FixedRoutes } from '@urls';
import { map } from 'rxjs';

export const AuthGuard = (next: ActivatedRouteSnapshot) => {
    return inject(AuthService).isAuthenticated.pipe(map((isLoggedIn: boolean) => isLoggedIn ? true : createUrlTreeFromSnapshot(next, ['/', FixedRoutes.Auth])));
};
