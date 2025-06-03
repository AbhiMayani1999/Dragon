export interface PermissionConfig {
  title: string;
  navigationUrl: string;
  userNavigationUrl: string;
  userTypeNavigationUrl: string;
}

export interface PermissionDataModel {
  id: number;
  name?: string;
  username?: string;
  navigation: PermissionAssignment[];
  isUser: boolean;
}

export interface PermissionAssignment {
  // id: number;
  // parentNavigationId: number;
  name: string;
  code: string;
  isFull: boolean;
  isCreate: boolean;
  isEdit: boolean;
  isView: boolean;
  isDelete: boolean;
  userId?: number;
  userTypeId?: number;
  children: PermissionAssignment[];
}
