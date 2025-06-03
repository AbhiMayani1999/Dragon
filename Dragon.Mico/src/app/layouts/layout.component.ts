import { Component, OnDestroy, OnInit } from '@angular/core';
import { EventService } from '../core/services/event.service';
import {
  LAYOUT_VERTICAL, LAYOUT_HORIZONTAL, LAYOUT_MODE, LAYOUT_WIDTH,
  LAYOUT_POSITION, SIDEBAR_SIZE, SIDEBAR_COLOR, TOPBAR
} from './layouts.model';
import { NavService } from '../core/services/nav.service';
import { Subscription } from 'rxjs';
import { NotifyService } from '@core/services/notify.service';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})

/**
 * Layout Component
 */
export class LayoutComponent implements OnInit, OnDestroy {

  layoutType!: string;
  layoutMode!: string;
  layoutwidth!: string;
  layoutposition!: string;
  topbar!: string;
  sidebarcolor!: string;
  sidebarsize!: string;

  private changeSubscription: Subscription;
  private changeModeSubscription: Subscription;
  private changeWidthSubscription: Subscription;
  private changePositionSubscription: Subscription;
  private changeTopbarSubscription: Subscription;
  private changeSidebarColorSubscription: Subscription;
  private changeSidebarSizeSubscription: Subscription;

  constructor(private eventService: EventService, private navService: NavService, private notifyService: NotifyService) { notifyService.listenToNotify(); }

  ngOnInit() {
    this.layoutMode = LAYOUT_MODE;
    this.layoutType = LAYOUT_VERTICAL;
    this.layoutwidth = LAYOUT_WIDTH;
    this.layoutposition = LAYOUT_POSITION;
    this.sidebarcolor = SIDEBAR_COLOR;
    this.sidebarsize = SIDEBAR_SIZE;
    this.topbar = TOPBAR;

    this.LayoutMode(this.layoutMode);
    this.LayoutWidth(this.layoutwidth);
    this.LayoutPosition(this.layoutposition);
    this.Topbar(this.topbar);
    this.SidebarColor(this.sidebarcolor);
    this.SidebarSize(this.sidebarsize);

    this.changeSubscription = this.eventService.subscribe('changeLayout', (layout) => {
      this.layoutType = layout;
    });

    this.changeModeSubscription = this.eventService.subscribe('changeMode', (mode) => {
      this.layoutMode = mode;
      this.LayoutMode(this.layoutMode);
    });

    this.changeWidthSubscription = this.eventService.subscribe('changeWidth', (width) => {
      this.layoutwidth = width;
      this.LayoutWidth(this.layoutwidth);
    });

    this.changePositionSubscription = this.eventService.subscribe('changePosition', (position) => {
      this.layoutposition = position;
      this.LayoutPosition(this.layoutposition);
    });

    this.changeTopbarSubscription = this.eventService.subscribe('changeTopbar', (topbar) => {
      this.topbar = topbar;
      this.Topbar(this.topbar);
    });

    this.changeSidebarColorSubscription = this.eventService.subscribe('changeSidebarColor', (sidebarcolor) => {
      this.sidebarcolor = sidebarcolor;
      this.SidebarColor(this.sidebarcolor);
    });

    this.changeSidebarSizeSubscription = this.eventService.subscribe('changeSidebarSize', (sidebarsize) => {
      this.sidebarsize = sidebarsize;
      this.SidebarSize(this.sidebarsize);
    });

    this.navService.getNavigations();
  }

  isVerticalLayoutRequested() {
    return this.layoutType === LAYOUT_VERTICAL;
  }

  isHorizontalLayoutRequested() {
    return this.layoutType === LAYOUT_HORIZONTAL;
  }

  LayoutMode(mode: string) {
    switch (mode) {
      case "light":
        document.body.setAttribute("data-bs-theme", "light");
        document.body.setAttribute("data-topbar", "light");
        document.body.setAttribute("data-sidebar", "light");
        break;
      case "dark":
        document.body.setAttribute("data-sidebar", "dark");
        document.body.setAttribute("data-bs-theme", "dark");
        document.body.setAttribute("data-topbar", "dark");
        break;
      default:
        document.body.setAttribute("data-bs-theme", "light");
        document.body.setAttribute("data-topbar", "light");
        break;
    }

  }

  LayoutWidth(width: string) {
    switch (width) {
      case "light":
        document.body.setAttribute("data-layout-size", "fluid");
        break;
      case "boxed":
        document.body.setAttribute("data-layout-size", "boxed");
        break;
      default:
        document.body.setAttribute("data-layout-size", "light");
        break;
    }
  }

  LayoutPosition(position: string) {
    if (position === 'fixed') {
      document.body.setAttribute("data-layout-scrollable", "false");
    } else {
      document.body.setAttribute("data-layout-scrollable", "true");
    }
  }

  SidebarColor(color: string) {
    switch (color) {
      case "light":
        document.body.setAttribute('data-sidebar', 'light');
        break;
      case "dark":
        document.body.setAttribute("data-sidebar", "dark");
        break;
      case "brand":
        document.body.setAttribute("data-sidebar", "brand");
        break;
      default:
        document.body.setAttribute("data-sidebar", "light");
        break;
    }
  }

  Topbar(topbar: string) {
    switch (topbar) {
      case "light":
        document.body.setAttribute("data-topbar", "light");
        break;
      case "dark":
        document.body.setAttribute("data-topbar", "dark");
        break;
      default:
        document.body.setAttribute("data-topbar", "light");
        break;
    }
  }

  SidebarSize(size: string) {
    switch (size) {
      case "default":
        document.body.setAttribute('data-sidebar-size', 'lg');
        break;
      case "compact":
        document.body.setAttribute('data-sidebar-size', 'md');
        break;
      case "small":
        document.body.setAttribute('data-sidebar-size', 'sm');
        break;
      default:
        document.body.setAttribute('data-sidebar-size', 'lg');
        break;
    }
  }

  ngOnDestroy() {
    if (this.changeSubscription) { this.changeSubscription.unsubscribe(); }
    if (this.changeModeSubscription) { this.changeModeSubscription.unsubscribe(); }
    if (this.changeWidthSubscription) { this.changeWidthSubscription.unsubscribe(); }
    if (this.changePositionSubscription) { this.changePositionSubscription.unsubscribe(); }
    if (this.changeTopbarSubscription) { this.changeTopbarSubscription.unsubscribe(); }
    if (this.changeSidebarColorSubscription) { this.changeSidebarColorSubscription.unsubscribe(); }
    if (this.changeSidebarSizeSubscription) { this.changeSidebarSizeSubscription.unsubscribe(); }
  }
}
