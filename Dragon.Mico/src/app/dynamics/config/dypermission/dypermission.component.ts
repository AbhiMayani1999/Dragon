import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DataService } from '@core/services/data.service';
import { NgbAccordionModule, NgbNavModule } from '@ng-bootstrap/ng-bootstrap';
import { Modules } from '@urls';
import { PermissionAssignment, PermissionConfig, PermissionDataModel } from './dypermission.model';
import { Nav } from '@core/models/nav.model';

@Component({
  selector: 'dypermission',
  standalone: true,
  imports: [CommonModule, NgbNavModule, NgbAccordionModule, FormsModule],
  templateUrl: './dypermission.component.html',
  styleUrl: './dypermission.component.scss'
})
export class DypermissionComponent {
  public permissionConfig: PermissionConfig;
  public userTypeData: PermissionDataModel[];
  public userData: PermissionDataModel[];
  public navigation: Nav[];
  public selectedAssignment: PermissionDataModel;
  private isSelectedUser: boolean;
  public currentNavigations: PermissionAssignment[];
  public permissionType: string = "";
  constructor(public dataService: DataService) { }

  @Input() set config(data: PermissionConfig) {
    if (data) {
    }
    this.permissionConfig = {
      title: "Permissions",
      userNavigationUrl: Modules.UserNavigation,
      userTypeNavigationUrl: Modules.UserTypeNavigation,
      navigationUrl: Modules.Navigation,
    };
    this.processData();
  }

  processData() {
    this.userTypeData = null; this.userData = null; this.navigation = null;
    this.dataService.getData<PermissionDataModel[]>(this.permissionConfig.userTypeNavigationUrl).then(response => this.userTypeData = response.map((d) => { return { ...d, isUser: false } }));
    this.dataService.getData<PermissionDataModel[]>(this.permissionConfig.userNavigationUrl).then(response => this.userData = response.map((d) => { return { ...d, isUser: true } }));
    this.dataService.getData<Nav[]>(this.permissionConfig.navigationUrl).then(response => { this.navigation = response; });
  }

  onSelectAssignment(data: PermissionDataModel, isUser: boolean) {
    this.selectedAssignment = data;
    this.isSelectedUser = isUser;
    this.permissionType = isUser ? ` / User / ${data.username}` : ` / UserType / ${data.name}`;
    this.selectedAssignment = this.prepareAssignment(data);
  }

  prepareAssignment(data: PermissionDataModel): PermissionDataModel {
    const userNavigation: PermissionAssignment[] = JSON.parse(JSON.stringify(data.navigation));
    this.currentNavigations = [];
    this.navigation.forEach(d => {
      let permission: PermissionAssignment = { name: d.name, code: d.code, isFull: false, isCreate: false, isEdit: false, isView: false, isDelete: false, userId: data.id, userTypeId: data.id, children: [] };
      const navPermission = userNavigation.find(u => u.code === d.code);
      if (navPermission) {
        permission = {
          ...permission,
          isEdit: navPermission.isEdit,
          isView: navPermission.isView,
          isCreate: navPermission.isCreate,
          isDelete: navPermission.isDelete,
          isFull: navPermission.isEdit && navPermission.isView && navPermission.isCreate && navPermission.isDelete
        };
      }
      this.currentNavigations.push(permission);
    });
    return data;
  }


  onAssignAll(item: PermissionAssignment) {
    item.isCreate = item.isFull; item.isEdit = item.isFull; item.isView = item.isFull; item.isDelete = item.isFull;
    if (item.children) {
      item.children.forEach(d => { d.isCreate = item.isFull; d.isEdit = item.isFull; d.isView = item.isFull; d.isDelete = item.isFull; d.isFull = item.isFull })
    }
  }

  onCheckAssignAll(item: PermissionAssignment) {
    item.isFull = item.isEdit && item.isView && item.isCreate && item.isDelete;
  }

  handleSubmit() {
    this.dataService.postData<[]>(this.isSelectedUser ?
      this.permissionConfig.userNavigationUrl : this.permissionConfig.userTypeNavigationUrl, this.currentNavigations).then(
        () => this.processData());
  }
}
