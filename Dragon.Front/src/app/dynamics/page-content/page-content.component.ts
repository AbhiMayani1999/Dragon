import { Component } from '@angular/core';
import { PageService } from '@services/page.service';

@Component({
  selector: 'dy-page-content',
  templateUrl: './page-content.component.html',
  styleUrls: ['./page-content.component.css'],
})
export class PageContentComponent {
  constructor(public pageService: PageService) {}
}
