import { Component, Input } from '@angular/core';
import { TableConfig } from '../awesomedatatable.model';
import { ColumnConfig } from '../awesomedatatable.component';
import { DataService } from '@services/data.service';
import { Modules } from '@urls';

@Component({
  selector: 'aw-tableview',
  templateUrl: './tableview.component.html',
  styleUrls: ['./tableview.component.css'],
})
export class TableviewComponent {
  public columnConfig: ColumnConfig[];

  constructor(public dataService: DataService) { }

  @Input() tableConfig: TableConfig;
  @Input() set config(data: TableConfig) {
    this.tableConfig = data;
    this.columnConfig = [];
    this.tableConfig.fields.forEach((field) => {
      this.columnConfig.push(field[Object.keys(field)[0]]);
    });
    this.getData();
  }

  getData() {
    this.dataService.getData(`${Modules.Base}${this.tableConfig.dataurl}`).then(response => {

    });
  }
}
