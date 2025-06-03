import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { PageService } from '@core/services/page.service';
import { DybreadcrumbComponent } from './dybreadcrumb/dybreadcrumb.component';
import { DycruddashboardComponent } from './dycruddashboard/dycruddashboard.component';
import { DynamicsDirective } from './dynamics.directive';

@Component({
  selector: 'app-dynamics',
  standalone: true,
  imports: [CommonModule, DybreadcrumbComponent, DynamicsDirective, DycruddashboardComponent],
  templateUrl: './dynamics.component.html',
  styleUrl: './dynamics.component.scss'
})
export class DynamicsComponent {
  constructor(public pageService: PageService) { }
}
