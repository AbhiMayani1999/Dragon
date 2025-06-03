import { Injectable, OnDestroy } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { NavigationEnd, Router } from '@angular/router';
import { PageConfig } from '@core/models/page.model';
import { Modules } from '@urls';
import { Subscription, filter } from 'rxjs';
import { AuthService } from './auth.service';
import { DataService } from './data.service';
import { NavService } from './nav.service';
import { Nav } from '@core/models/nav.model';

@Injectable({ providedIn: 'root' })
export class PageService implements OnDestroy {
  private routerSubscription: Subscription;
  public breadcrumb: Nav[];
  public currentNavigation: Nav | undefined;
  public pageConfig: PageConfig | any;

  constructor(
    private title: Title,
    private router: Router,
    private navService: NavService,
    private dataService: DataService,
    private authService: AuthService) { this.startConfig(); }

  public startConfig() {
    if (!this.routerSubscription) { this.preparePageConfig(this.router.url); }
    this.routerSubscription = this.router.events.pipe(filter(event => event instanceof NavigationEnd)).subscribe(() => {
      this.preparePageConfig(this.router.url);
    });
  }

  private preparePageConfig(url: string) {
    const navParts = url.split('/').filter((d) => d != '');
    this.currentNavigation = undefined;
    this.breadcrumb = [];
    this.title.setTitle('');
    if (this.navService.currentNavigations) {
      if (!navParts.length) { this.router.navigateByUrl(this.navService.currentNavigations[0].cstate); }
      else {
        const navigation = this.navService.currentNavigations.find((d) => d.state === navParts[navParts.length - 1]);
        if (navigation && navigation.isView) {
          this.title.setTitle(navigation.name);
          this.currentNavigation = navigation;
          navParts.forEach((nav) => this.breadcrumb.push(this.navService.currentNavigations.find((d) => d.state === nav) ?? { name: '' }));
          this.getPageUI(url);
        }
      }
    }
  }

  private getPageUI(url: string) {
    this.pageConfig = null;
    this.dataService.get(`${Modules.FileUrl}/${this.authService.currentUser.tenantCode}/PageConfig${url}.json`)
      .then((response) => {
        this.pageConfig = response;
      });
  }

  ngOnDestroy(): void {
    if (this.routerSubscription) {
      this.routerSubscription.unsubscribe();
    }
  }
}
