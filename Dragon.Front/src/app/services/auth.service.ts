import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { FixedRoutes, Modules } from '@urls';
import { jwtDecode } from "jwt-decode";
import { BehaviorSubject } from 'rxjs';
import { DataService } from './data.service';
import { User } from './model/auth.model';
import { ApiResponse, StatusFlags } from './model/data.service.model';

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
                    this.router.navigateByUrl(FixedRoutes.Dashboard);
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
                this.isAuthenticated.next(true);
                isDecoded = true;
            }
        }
        return isDecoded;
    }
}
