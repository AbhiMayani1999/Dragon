import { Injectable } from "@angular/core";
import { Modules } from "src/app/app.urls";
import { DataService } from "./data.service";
import { ApiResponse, StatusFlags } from "./model/data.service.model";
import { Navigation } from "./model/nav.model";

@Injectable({ providedIn: 'root' })
export class NavService {
  public currentNavigations: Navigation[];
  public preparedNavigations: Navigation[];

  constructor(private dataService: DataService) { this.processNavigations(); }

  public getNavigations() {
    if (!sessionStorage.Navigation) {
      this.dataService.get<Navigation[]>(`${Modules.Config.MyNavigations}`).then((response: ApiResponse<Navigation[]>) => {
        if (response.status === StatusFlags.Success && response.data && response.data.length) {
          sessionStorage.Navigation = JSON.stringify(response.data);
          this.processNavigations();
        }
      });
    }
  }

  private processNavigations() {
    if (sessionStorage.Navigation) {
      this.currentNavigations = JSON.parse(sessionStorage.Navigation);
      this.preparedNavigations = this.currentNavigations.filter((d) => !d.parentNavigationId || d.parentNavigationId == null);
      this.preparedNavigations.forEach((nav) => (nav.childItems = this.recurringTransformation(nav, this.currentNavigations)));
    }
  }

  private recurringTransformation(nav: Navigation, navigationList: Navigation[]): Navigation[] {
    const children = navigationList.filter((d) => d.parentNavigationId === nav.id);
    children.forEach((child) => { child.childItems = this.recurringTransformation(child, navigationList); });
    return children;
  }
}
// child.state = nav.state ? `/${nav.state}/${child.state}` : `/${child.state}`;
