import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { Component } from '@angular/core';
import { PageService } from '@core/services/page.service';

@Component({
  selector: 'dybreadcrumb',
  standalone: true,
  imports: [CommonModule, HttpClientModule],
  templateUrl: './dybreadcrumb.component.html',
  styleUrl: './dybreadcrumb.component.scss'
})
export class DybreadcrumbComponent {
  constructor(public pageService: PageService) { }
}
