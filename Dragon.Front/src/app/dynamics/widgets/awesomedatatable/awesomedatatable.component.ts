import { Component, Input } from '@angular/core';
import { TableConfig } from './awesomedatatable.model';

@Component({
  selector: 'dy-awesomedatatable',
  templateUrl: './awesomedatatable.component.html',
  styleUrls: ['./awesomedatatable.component.css'],
})
export class AwesomedatatableComponent {
  @Input() public config: TableConfig;
  public isFrom: boolean = false;

  // @Input() set config(data: TableConfig) {
  //   this.tableConfig = data;
  //   this.columnConfig = [];
  //   this.tableConfig.fields.forEach((field) => {
  //     this.columnConfig.push(field[Object.keys(field)[0]]);
  //   });
  // }

  public tableData = [

  ];
}

export interface ColumnConfig {
  jsonProperty: string;
  title: string;
}