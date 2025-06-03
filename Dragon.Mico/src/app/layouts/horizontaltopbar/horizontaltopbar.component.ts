
import { AfterViewInit, Component, EventEmitter, OnInit, Output } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';

import { TranslateService } from '@ngx-translate/core';
import { CookieService } from 'ngx-cookie-service';

import { AuthService } from '../../core/services/auth.service';
import { EventService } from '../../core/services/event.service';
import { LanguageService } from '../../core/services/language.service';
import { LAYOUT_MODE } from "../layouts.model";
import { FixedRoutes } from 'src/app/app.urls';
import { NavService } from 'src/app/core/services/nav.service';
import { Nav } from 'src/app/core/models/nav.model';

@Component({
  selector: 'app-horizontaltopbar',
  templateUrl: './horizontaltopbar.component.html',
  styleUrls: ['./horizontaltopbar.component.scss'],
})

export class HorizontaltopbarComponent implements OnInit, AfterViewInit {

  mode: string | undefined;
  layoutMode!: string;

  navigations: Nav[] = [];
  element: any;
  flagvalue: any;
  cookieValue: any;
  countryName: any;
  valueset: any;

  /**
   * Language Listing
   */
  listLang = [
    { text: 'English', flag: 'assets/images/flags/us.jpg', lang: 'en' },
    { text: 'Spanish', flag: 'assets/images/flags/spain.jpg', lang: 'es' },
    { text: 'German', flag: 'assets/images/flags/germany.jpg', lang: 'de' },
    { text: 'Italian', flag: 'assets/images/flags/italy.jpg', lang: 'it' },
    { text: 'Russian', flag: 'assets/images/flags/russia.jpg', lang: 'ru' },
  ];

  @Output() settingsButtonClicked = new EventEmitter();
  @Output() mobileMenuButtonClicked = new EventEmitter();

  constructor(
    private router: Router,
    public navService: NavService,
    public translate: TranslateService,
    public languageService: LanguageService,
    public cookiesService: CookieService,
    private eventService: EventService,
    private authService: AuthService,
  ) {
    router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.activateMenu();
      }
    });
  }

  setLanguage(text: string, lang: string, flag: string) {
    this.countryName = text;
    this.flagvalue = flag;
    this.cookieValue = lang;
    this.languageService.setLanguage(lang);
  }

  ngOnInit(): void {
    this.element = document.documentElement;
    this.layoutMode = LAYOUT_MODE;
    this.cookieValue = this.cookiesService.get('lang');
    const val = this.listLang.filter(x => x.lang === this.cookieValue);
    this.countryName = val.map(element => element.text);
    if (val.length === 0) {
      if (this.flagvalue === undefined) { this.valueset = 'assets/images/flags/us.jpg'; }
    } else {
      this.flagvalue = val.map(element => element.flag);
    }
  }

  hasItems(item: Nav) {
    return item.childItems !== undefined ? item.childItems.length > 0 : false;
  }

  onSettingsButtonClicked() {
    document.body.classList.toggle('right-bar-enabled');
  }

  onMenuClick(event: any) {
    const nextEl = event.target.nextElementSibling;
    if (nextEl) {
      const parentEl = event.target.parentNode;
      if (parentEl) {
        parentEl.classList.remove('show');
      }
      nextEl.classList.toggle('show');
    }
    return false;
  }

  ngAfterViewInit() {
    this.activateMenu();
  }

  removeAllClass(className: any) {
    const els = document.getElementsByClassName(className);
    while (els[0]) {
      els[0].classList.remove(className);
    }
  }

  changeMode(mode: string) {
    this.mode = mode;
    this.layoutMode = mode;
    this.eventService.broadcast('changeMode', mode);
  }

  toggleMobileMenu(event: any) {
    event.preventDefault();
    this.mobileMenuButtonClicked.emit();
  }

  toggleRightSidebar() {
    this.settingsButtonClicked.emit();
  }

  private activateMenu() {
    const resetParent = (el: any) => {
      const parent = el.parentElement;
      if (parent) {
        parent.classList.remove('active');
        const parent2 = parent.parentElement;
        this.removeAllClass('mm-active');
        this.removeAllClass('mm-show');
        if (parent2) {
          parent2.classList.remove('active');
          const parent3 = parent2.parentElement;
          if (parent3) {
            parent3.classList.remove('active');
            const parent4 = parent3.parentElement;
            if (parent4) {
              parent4.classList.remove('active');
              const parent5 = parent4.parentElement;
              if (parent5) {
                parent5.classList.remove('active');
                const menuelement = document.getElementById(
                  'topnav-menu-content'
                );
                if (menuelement !== null)
                  if (menuelement.classList.contains('show'))
                    document
                      .getElementById('topnav-menu-content')!
                      .classList.remove('show');
              }
            }
          }
        }
      }
    };

    const links: any = document.getElementsByClassName('side-nav-link-ref');
    let matchingMenuItem = null;
    for (let i = 0; i < links.length; i++) {
      resetParent(links[i]);
    }
    for (let i = 0; i < links.length; i++) {
      if (location.pathname === links[i]['pathname']) {
        matchingMenuItem = links[i];
        break;
      }
    }

    if (matchingMenuItem) {
      const parent = matchingMenuItem.parentElement;
      /**
       * TODO: This is hard coded way of expading/activating parent menu dropdown and working till level 3.
       * We should come up with non hard coded approach
       */
      if (parent) {
        parent.classList.add('active');
        const parent2 = parent.parentElement;
        if (parent2) {
          parent2.classList.add('active');
          const parent3 = parent2.parentElement;
          if (parent3) {
            parent3.classList.add('active');
            const parent4 = parent3.parentElement;
            if (parent4) {
              parent4.classList.add('active');
              const parent5 = parent4.parentElement;
              if (parent5) {
                parent5.classList.add('active');
              }
            }
          }
        }
      }
    }
  }

  logout() {
    this.authService.logout();
  }
}
