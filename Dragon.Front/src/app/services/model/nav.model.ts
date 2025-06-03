export interface Navigation {
  id?: number;
  state?: string;
  name: string;
  sortIndex?: number;
  icon?: string;
  parentNavigationId?: number;
  childItems?: Navigation[];

  isCreate?: boolean;
  isEdit?: boolean;
  isView?: boolean;
  isDelete?: boolean;
  isLayout?: boolean;
  badge?: any
}