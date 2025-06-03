import { OptionTransfer } from "@core/models/dyna.model";

export interface TableConfig {
  title: string;
  dataurl?: string;
  form?: any;
  fields?: [];
  editAction?: string;
  deleteAction?: string;
  parentIdentifier?: number;
  serverSidePagination?: boolean;
}

export interface ColumnConfig {
  jsonProperty: string;
  title: string;

  identifier: boolean;
  optionTransfer?: OptionTransfer;
  isImage: boolean;
  isDocument: boolean;
}

export interface TablePageData {
  pageSize: number;
  currentPage: number;
  recordsCount: number;
  isClientSide: boolean;
  data: any[];
  filter: any;
}

export enum TableColumnNames {
  ImageColumn = 'imageColumn',
  DocumentColumn = 'documentColumn'
}
