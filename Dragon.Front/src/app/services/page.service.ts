import { Injectable, OnDestroy } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { NavigationEnd, Router } from '@angular/router';
import { Modules } from '@urls';
import { Subscription } from 'rxjs';
import { AuthService } from './auth.service';
import { DataService } from './data.service';
import { Navigation } from './model/nav.model';
import { PageConfig } from './model/page.service.model';
import { NavService } from './nav.service';

@Injectable({ providedIn: 'root' })
export class PageService implements OnDestroy {
  private routerSubscription: Subscription;
  public breadcrumb: Navigation[];
  public currentNavigation: Navigation | undefined;
  public pageConfig: PageConfig | any;

  constructor(
    private title: Title,
    private router: Router,
    private navService: NavService,
    private dataService: DataService,
    private authService: AuthService) {
    this.routerSubscription = this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.preparePageConfig(this.router.url);
      }
    });
    this.preparePageConfig(this.router.url);
  }

  private preparePageConfig(url: string) {
    const navParts = url.split('/').filter((d) => d != '');
    this.currentNavigation = undefined;
    this.breadcrumb = [];
    this.title.setTitle('');
    if (this.navService.currentNavigations) {
      const navigation = this.navService.currentNavigations.find((d) => d.state === navParts[navParts.length - 1]);
      if (navigation && navigation.isView) {
        this.title.setTitle(navigation.name);
        this.currentNavigation = navigation;
        navParts.forEach((nav) => this.breadcrumb.push(this.navService.currentNavigations.find((d) => d.state === nav) ?? { name: '' }));
        this.getPageUI(url);
      }
    }
  }

  private getPageUI(url: string) {
    this.pageConfig = { state: '', title: '', widgets: [] };
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
