import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { Component, Input, OnDestroy, OnInit, forwardRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DataService } from '@core/services/data.service';
import { OptionService } from '@core/services/option.service';
import { NgbModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { Modules, RouteExtensions } from '@urls';
import { Subscription } from 'rxjs';
import Swal from 'sweetalert2';
import { DycrudformComponent } from '../dycrudform/dycrudform.component';
import { FromConfig } from '../dycrudform/dycrudfrom.model';
import { ColumnConfig, TableColumnNames, TableConfig, TablePageData } from './dycrudtable.model';
import { FileIconComponent } from '../dycrudform/uploadfield/file-icon/file-icon.component';
import { TablefilterPipe } from '@core/pipes/tablefilter.pipe';

@Component({
  selector: 'app-dycrudtable',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule, NgbModule, NgbPaginationModule, DycrudtableComponent, TablefilterPipe, FileIconComponent, forwardRef(() => DycrudformComponent)],
  templateUrl: './dycrudtable.component.html',
  styleUrl: './dycrudtable.component.scss',
})
export class DycrudtableComponent implements OnInit, OnDestroy {
  @Input() public config: TableConfig;
  public formConfig: FromConfig;
  public isFrom: boolean = false;
  public cardView: boolean = false;

  public tableData: any[];
  public parentFilter = {};
  public originalTableData: any[];
  public columnConfig: ColumnConfig[];
  public pageConfig: TablePageData = null;

  public imageUrl = `${Modules.Images.Tenant}/${sessionStorage.TenantCode}/${RouteExtensions.Image}/`;
  private dynaOptionSubscription: Subscription;

  constructor(public dataService: DataService, public optionService: OptionService) { }

  ngOnInit(): void {
    this.onCardView();
    this.processTable();
  }

  onCardView() {
    if (!this.cardView) {
      this.cardView = window.innerWidth < 992;
    }
  }

  processTable() {
    this.columnConfig = [];
    if (this.config && this.config.fields && this.config.fields.length) {
      if (this.config.form) { this.formConfig = { ...this.config.form }; }
      this.config.fields.forEach((field) => {
        const fieldType = Object.keys(field)[0];
        const fieldConfig: ColumnConfig = field[fieldType];
        fieldConfig.isImage = fieldType === TableColumnNames.ImageColumn;
        fieldConfig.isDocument = fieldType === TableColumnNames.DocumentColumn;
        if (!fieldConfig.identifier) { this.columnConfig.push(fieldConfig); } else { this.parentFilter[fieldConfig.jsonProperty] = this.config.parentIdentifier; if (this.formConfig) { this.formConfig.parentIdentifier = this.config.parentIdentifier; } }
        if (fieldConfig.optionTransfer) { this.optionService.optionTransferList.push(fieldConfig.optionTransfer); }
      });
      if (this.optionService.optionTransferList && this.optionService.optionTransferList.length) { this.getOption(); }
    }
    this.getData();
  }

  onOpenForm(data?: any) {
    this.formConfig.data = data ? this.originalTableData.find(d => d.id == data.id) : null;
    this.isFrom = true;
  }

  onCloseForm(event: boolean): void {
    this.isFrom = false;
    if (event) { this.getOption(); this.getData(); }
  }

  getOption() {
    if (!this.dynaOptionSubscription) {
      this.dynaOptionSubscription = this.optionService.optionSubject.subscribe(data => {
        if (data) {
          this.columnConfig.filter(d => d.optionTransfer).forEach(item => {
            const optionTransfer = this.optionService.optionTransferList.find(o =>
              (o.table === item.optionTransfer.table && o.key === item.optionTransfer.key && o.value === item.optionTransfer.value) ||
              o.keystore === item.optionTransfer.keystore);
            if (optionTransfer) { item.optionTransfer.options = optionTransfer.options; }
          });
          this.processViewData(this.tableData);
        }
      });
    }
    this.optionService.processTransfer();
  }

  getData() {
    this.dataService.postData<TablePageData>(`${Modules.Base}${this.config.dataurl}/${RouteExtensions.GetPage}`,
      this.pageConfig ? this.pageConfig : { filter: this.parentFilter, isClientSide: true })
      .then(response => { this.originalTableData = JSON.parse(JSON.stringify(response.data)); this.processViewData(response.data); this.pageConfig = response; delete this.pageConfig.data; });
  }

  onDeleteRecord(item: number) {
    Swal.fire({
      icon: 'warning',
      title: 'Are you sure?',
      text: 'You won\'t be able to revert deleted record..!',
      showCancelButton: true, confirmButtonColor: '#34c38f', cancelButtonColor: '#f46a6a',
      confirmButtonText: 'Yes, delete it!'
    }).then(result => {
      if (result.value) { this.dataService.delete(`${Modules.Base}${this.config.dataurl}`, item).then(() => this.getData()); }
    });
  }

  processViewData(data: any[]) {
    if (data && data.length) {
      this.columnConfig.filter(d => d.optionTransfer).forEach(column => {
        data.forEach(item => {
          if (item[column.jsonProperty] && column.optionTransfer && column.optionTransfer.options) {
            item[column.jsonProperty] = column.optionTransfer.options.find(d => d.key == item[column.jsonProperty])?.value;
          }
        });
      });
      this.tableData = data;
    }
  }

  ngOnDestroy(): void {
    if (this.dynaOptionSubscription) { this.dynaOptionSubscription.unsubscribe(); }
  }
}

// let prepareFilter = false;
// if (!this.pageConfig) { this.pageConfig = { currentPage: 0, isClientSide: false, pageSize: 12, recordsCount: 0, data: [], filter: {} }; prepareFilter = true; }
// if (prepareFilter) { this.pageConfig.filter[fieldConfig.jsonProperty] = fieldConfig.identifier ? '' : ''; }
