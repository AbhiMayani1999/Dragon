export interface User {
    id: number;
    username: string;
    password: string;
    isActive: boolean;
    isDeleted: boolean;
    userTypeId: number;
    userType: UserType;
    tenantCode: string;
    accessToken: string;
    userTypeName: any;
    createdBy: number;
    createdDate: string;
    updatedBy: any;
    updatedDate: string;
}

export interface UserType {
    id: number;
    name: string;
    isAdmin: boolean;
    isActive: boolean;
    isDeleted: boolean;
    createdBy: number;
    createdDate: string;
    updatedBy: any;
    updatedDate: string;
}
