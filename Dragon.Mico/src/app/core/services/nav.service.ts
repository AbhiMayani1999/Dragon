import { Injectable } from "@angular/core";
import { Modules } from "src/app/app.urls";
import { ApiResponse, StatusFlags } from "../models/data.model";
import { Nav } from "../models/nav.model";
import { DataService } from "./data.service";

@Injectable({ providedIn: 'root' })

export class NavService {
    public currentNavigations: Nav[];
    public preparedNavigations: Nav[];

    constructor(private dataService: DataService) { this.processNavigations(); }

    public getNavigations() {
        if (!sessionStorage.Navigation) {
            this.dataService.get<Nav[]>(`${Modules.Config.MyNavigations}`).then((response: ApiResponse<Nav[]>) => {
                if (response.status === StatusFlags.Success && response.data && response.data.length) {
                    sessionStorage.Navigation = JSON.stringify(response.data);
                    this.processNavigations();
                }
            });
        }
    }

    private processNavigations(): boolean {
        let isDecoded = false;
        if (sessionStorage.Navigation) {
            this.currentNavigations = JSON.parse(sessionStorage.Navigation);
            this.preparedNavigations = this.currentNavigations.filter((d) => !d.parentNavigationId || d.parentNavigationId == null);
            this.preparedNavigations.forEach((nav) => (nav.childItems = this.recurringTransformation(nav, this.currentNavigations)));
        }
        return isDecoded;
    }

    private recurringTransformation(nav: Nav, navigationList: Nav[]): Nav[] {
        const children = navigationList.filter((d) => d.parentNavigationId === nav.id);
        children.forEach((child) => {
            child.cstate = nav.state ? `/${nav.state}/${child.state}` : `/${child.state}`;
            child.childItems = this.recurringTransformation(child, navigationList);
        });
        return children;
    }
}
