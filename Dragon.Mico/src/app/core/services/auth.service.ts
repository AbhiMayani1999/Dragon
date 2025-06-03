import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { jwtDecode } from "jwt-decode";
import { BehaviorSubject } from 'rxjs';
import { FixedRoutes, Modules } from 'src/app/app.urls';
import { User } from '../models/auth.model';
import { ApiResponse, StatusFlags } from '../models/data.model';
import { DataService } from './data.service';

@Injectable({ providedIn: 'root' })
export class AuthService {
  public currentUser: User;
  public isAuthenticated: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

  constructor(private dataService: DataService, private router: Router) { this.decodeToken(); }

  public login(user: { username: string, password: string }): void {
    this.dataService.post<string>(`${Modules.Auth}`, user).then((response: ApiResponse<string>) => {
      if (response.status === StatusFlags.Success && response.data) {
        sessionStorage.AccessToken = response.data;
        if (this.decodeToken()) {
          this.router.navigateByUrl(FixedRoutes.Root);
        }
      }
    });
  }

  public logout(): void {
    sessionStorage.clear();
    this.isAuthenticated.next(false);
    this.router.navigateByUrl(FixedRoutes.Auth);
  }

  private decodeToken(): boolean {
    let isDecoded = false;
    if (sessionStorage.AccessToken) {
      const decodedToken = jwtDecode(sessionStorage.AccessToken);
      const userdata = decodedToken[Object.keys(decodedToken).find(d => d.includes("userdata"))];
      if (userdata) {
        this.currentUser = JSON.parse(userdata);
        sessionStorage.TenantCode = this.currentUser.tenantCode;
        this.isAuthenticated.next(true);
        isDecoded = true;
      }
    }
    return isDecoded;
  }
}

// resetPassword(email: string) {
//     // return getFirebaseBackend()!.forgetPassword(email).then((response: any) => {
//     //     const message = response.data;
//     //     return message;
//     // });
// }

// register(email: string, password: string) {
//     // return getFirebaseBackend()!.registerUser(email, password).then((response: any) => {
//     //     const user = response;
//     //     return user;
//     // });
// }
