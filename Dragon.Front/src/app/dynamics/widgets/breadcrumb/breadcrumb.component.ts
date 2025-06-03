import { Component } from '@angular/core';
import { PageService } from '@services/page.service';

@Component({
  selector: 'dy-breadcrumb',
  templateUrl: './breadcrumb.component.html',
  styleUrls: ['./breadcrumb.component.css'],
})
export class BreadcrumbComponent {
  constructor(public pageService: PageService) {}
}
