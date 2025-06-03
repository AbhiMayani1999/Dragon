import { Component } from '@angular/core';
import { NgbNavModule } from '@ng-bootstrap/ng-bootstrap';
import { ChartType, NgApexchartsModule } from 'ng-apexcharts';

@Component({
  selector: 'dycruddashboard',
  standalone: true,
  imports: [NgbNavModule, NgApexchartsModule],
  templateUrl: './dycruddashboard.component.html',
  styleUrl: './dycruddashboard.component.scss'
})
export class DycruddashboardComponent {
  marketOverview!: any;
}
