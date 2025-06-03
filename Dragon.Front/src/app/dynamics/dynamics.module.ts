import { DragDropModule } from '@angular/cdk/drag-drop';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { DataTableModule } from '@bhplugin/ng-datatable';
import { SortablejsModule } from '@dustfoundation/ngx-sortablejs';
import { NgSelectModule } from '@ng-select/ng-select';
import { PageService } from '@services/page.service';
import { ModalModule } from 'angular-custom-modal';
import { IconModule } from '../core/icon/icon.module';
import { DyAlertComponent } from './components/dy-alert/dy-alert.component';
import { PageContentComponent } from './page-content/page-content.component';
import { PageContentDirective } from './page-content/page-content.directive';
import { AwesomedatatableComponent } from './widgets/awesomedatatable/awesomedatatable.component';
import { CardviewComponent } from './widgets/awesomedatatable/cardview/cardview.component';
import { TableviewComponent } from './widgets/awesomedatatable/tableview/tableview.component';
import { AwesomeformfieldDirective } from './widgets/awesomefrom/awesomeformfield.directive';
import { AwesomefromComponent } from './widgets/awesomefrom/awesomefrom.component';
import { NumberfieldComponent } from './widgets/awesomefrom/components/numberfield/numberfield.component';
import { SelectfieldComponent } from './widgets/awesomefrom/components/selectfield/selectfield.component';
import { TextfieldComponent } from './widgets/awesomefrom/components/textfield/textfield.component';
import { BreadcrumbComponent } from './widgets/breadcrumb/breadcrumb.component';
import { ComponentGeneratorComponent } from './widgets/generator/component-generator/component-generator.component';
import { GeneratorComponent } from './widgets/generator/generator.component';
import { PropertyGeneratorComponent } from './widgets/generator/property-generator/property-generator.component';
import { EmailfieldComponent } from './widgets/awesomefrom/components/emailfield/emailfield.component';
import { DatefieldComponent } from './widgets/awesomefrom/components/datefield/datefield.component';
import { CheckboxfieldComponent } from './widgets/awesomefrom/components/checkboxfield/checkboxfield.component';
import { Ng2FlatpickrModule } from 'ng2-flatpickr';

@NgModule({
  declarations: [
    GeneratorComponent,
    ComponentGeneratorComponent,
    PropertyGeneratorComponent,
    DyAlertComponent,
    PageContentComponent,
    BreadcrumbComponent,
    PageContentDirective,
    AwesomedatatableComponent,
    TableviewComponent,
    CardviewComponent,
    AwesomefromComponent,
    TextfieldComponent,
    NumberfieldComponent,
    SelectfieldComponent,
    AwesomeformfieldDirective,
    EmailfieldComponent,
    DatefieldComponent,
    CheckboxfieldComponent,
  ],
  imports: [
    RouterModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    DataTableModule,
    ModalModule,
    IconModule,
    SortablejsModule,
    DragDropModule,
    NgSelectModule,
    Ng2FlatpickrModule
  ],
  exports: [PageContentComponent, BreadcrumbComponent],
  providers: [PageService],
})
export class DynamicsModule { }
