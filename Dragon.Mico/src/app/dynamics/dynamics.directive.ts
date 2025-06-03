import { Directive, Input, ViewContainerRef } from '@angular/core';
import { DycrudtableComponent } from './dycrudtable/dycrudtable.component';
import { DypermissionComponent } from './config/dypermission/dypermission.component';
import { DycalanderComponent } from './widgets/dycalander/dycalander.component';

@Directive({ selector: 'dynamicsdirective', standalone: true })
export class DynamicsDirective {
  componentRef: any;
  constructor(private container: ViewContainerRef) { }

  @Input() set widget(data: any) {
    const componentType = Object.keys(data)[0];
    const widget = DynamicsMapper[componentType];
    if (widget) {
      this.componentRef = this.container.createComponent(widget);
      this.componentRef.instance.config = data[componentType];
    } else {
      console.warn(`Component type not found: ${componentType}`);
    }
  }
}

export enum WidgetNames {
  // Generator = 'generator',
  Table = 'table',
  UserPermission = 'userPermission',
  Calendar = 'calendar'
}

export const DynamicsMapper = {
  // [WidgetNames.Generator]: GeneratorComponent,
  [WidgetNames.Table]: DycrudtableComponent,
  [WidgetNames.UserPermission]: DypermissionComponent,
  [WidgetNames.Calendar]: DycalanderComponent,
};
