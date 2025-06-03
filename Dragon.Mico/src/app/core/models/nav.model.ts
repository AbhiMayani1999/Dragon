export interface Nav {
  id?: number;
  code?: string;
  state?: string;
  name: string;
  sortIndex?: number;
  icon?: string;
  parentNavigationId?: number;
  childItems?: Nav[];

  isCreate?: boolean;
  isEdit?: boolean;
  isView?: boolean;
  isDelete?: boolean;
  isLayout?: boolean;
  badge?: any;
  cstate?: string;
}
