import { Component } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { AppService } from '@services/app.service';
import { DataService } from '@services/data.service';
import { Navigation } from '@services/model/nav.model';
import { NavService } from '@services/nav.service';
import { Modules } from '@urls';

@Component({
  selector: 'app-manager',
  templateUrl: './manager.component.html',
  styleUrls: ['./manager.component.css'],
})
export class ManagerComponent {
  store: any;
  showTopButton = false;
  constructor(
    private router: Router,
    private service: AppService,
    public storeData: Store<any>,
    private dataService: DataService,
    public translate: TranslateService,
    private navService: NavService
  ) {
    this.initStore();
    this.navService.getNavigations();
  }

  headerClass = '';

  ngOnInit() {
    this.initAnimation();
    this.toggleLoader();
    this.initScroll();
  }

  initScroll() {
    window.addEventListener('scroll', () => {
      if (document.body.scrollTop > 50 || document.documentElement.scrollTop > 50) {
        this.showTopButton = true;
      } else {
        this.showTopButton = false;
      }
    });
  }

  initAnimation() {
    this.service.changeAnimation();
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.service.changeAnimation();
      }
    });

    const ele: any = document.querySelector('.animation');
    ele.addEventListener('animationend', () => {
      this.service.changeAnimation('remove');
    });
  }

  toggleLoader() {
    this.storeData.dispatch({ type: 'toggleMainLoader', payload: true });
    setTimeout(() => {
      this.storeData.dispatch({ type: 'toggleMainLoader', payload: false });
    }, 500);
  }

  async initStore() {
    this.storeData.select((d) => d.index).subscribe((d) => (this.store = d));
  }

  goToTop() {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
  }

  ngOnDestroy() {
    window.removeEventListener('scroll', () => { });
  }
}
