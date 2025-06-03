import { moveItemInArray } from '@angular/cdk/drag-drop';
import { Component, Input, ViewChild } from '@angular/core';
import { DataService } from '@services/data.service';
import { newRandom } from '@services/utiltiy.service';
import { Modules, RouteExtension } from '@urls';
import { DyAlertComponent } from '../../components/dy-alert/dy-alert.component';
import { AlertConfig } from '../../models/dy-alert.model';
import { ComponentBreadcrumb, ComponentStructure, StructureIndex, StructureStore, StructureStoreProperty } from '@services/model/dynamics/generator.model';
import { ComponentGeneratorComponent } from './component-generator/component-generator.component';
import { PropertyGeneratorComponent } from './property-generator/property-generator.component';

@Component({
  selector: 'dy-generator',
  templateUrl: './generator.component.html',
  styleUrls: ['./generator.component.css'],
})
export class GeneratorComponent {
  @ViewChild('componentGenerator', { static: true }) componentGenerator: ComponentGeneratorComponent;
  @ViewChild('propertyGenerator', { static: true }) propertyGenerator: PropertyGeneratorComponent;
  @ViewChild('alertModal', { static: true }) alertModal: DyAlertComponent;
  @Input() config: any;

  public componentOriginalList: ComponentStructure[] = [];
  public componentVisibleList: ComponentStructure[] = [];
  public parentComponentList: ComponentStructure[] = [];

  public storeOriginalList: StructureStore[] = [];
  public storeVisibleList: StructureStore[] = [];
  public storeVisibleListIndex: StructureIndex[] = [];
  public currentParentId: number = 0;

  public breadcrumbs: ComponentBreadcrumb[] = [];

  public editStructure!: StructureStore;

  public editProperty!: StructureStoreProperty | null;
  private onChangeTimeOut: NodeJS.Timeout;

  public alertConfig: AlertConfig = {
    title: 'Delete',
    description: 'Are you sure want to delete?',
    confirmBtnTitle: 'Delete',
    cancelBtnTitle: 'Cancel',
  };

  constructor(private dataService: DataService) {
    this.getGeneratorData();
  }

  public getGeneratorData(): void {
    this.getComponentStructures();
    this.getStructuresStore();
  }

  public getComponentStructures(): void {
    this.dataService.getData<ComponentStructure[]>(`${Modules.ComponentStructure}`).then((response: ComponentStructure[]) => {
      if (response && response.length) {
        this.componentOriginalList = this.processComponents(response);
        this.parentComponentList = this.componentOriginalList.filter((d) => d.canBeParent);
        const parentId = this.breadcrumbs.length ? this.breadcrumbs[this.breadcrumbs.length - 1].componentId : 0;
        this.filterComponentByParentMapping(parentId);
      }
    });
  }

  public getStructuresStore(): void {
    this.dataService.getData<StructureStore[]>(`${Modules.StructureStore}`).then((response: StructureStore[]) => {
      if (response && response.length) {
        this.storeOriginalList = this.processStores(response);
        this.filterStructureByParentMapping(this.currentParentId);
      }
    });
  }

  public processComponents(data: ComponentStructure[]): ComponentStructure[] {
    data.forEach((component) => {
      component.propertyNameList = component.properties.map((p) => p.componentProperty?.name).join(' | ');
      component.subComponentNameList = data
        .filter((d) => component.childMapping?.includes(d.id))
        ?.map((c) => c.name)
        .join(' | ');
    });
    return data;
  }

  public processStores(data: StructureStore[]): StructureStore[] {
    data.forEach((store) => {
      store.propertyNameList = store.properties.map((property) => property.componentStructureProperty?.componentProperty?.name).join(' | ');
      store.TitleProperty = store.properties.find((property) => property.componentStructureProperty?.componentProperty?.name === "Title")?.value;
      store.subStructureNameList = data
        .filter((d) => store.childMapping?.includes(d.id))
        ?.map((c) => `${c.properties.find((property) => property.componentStructureProperty?.componentProperty?.name === "Title")?.value ? c.properties.find((property) => property.componentStructureProperty?.componentProperty?.name === "Title")?.value : ''} ( ${c.componentStructure.name} ) `)
        .join(' | ');
      store.identifierEnable = false;
    });
    return data;
  }

  public filterComponentByParentMapping(data: number) {
    this.componentVisibleList =
      data === 0
        ? this.componentOriginalList.filter((d) => !d.parentMapping?.length)
        : this.componentOriginalList.filter((d) => d.parentMapping?.includes(data));
  }

  public filterStructureByParentMapping(data: number) {
    this.storeVisibleListIndex = [];
    this.storeVisibleList = data === 0 ? this.storeOriginalList.filter((d) => !d.parentMapping?.length) : this.storeOriginalList.filter((d) => d.parentMapping?.includes(data));

    this.storeVisibleList.forEach(d => this.storeVisibleListIndex.push({ id: d.id, shortIndex: d.shortIndex ? d.shortIndex : 0 }));
    this.onRegenerateIndex();
  }

  public onExpandStructure(data: StructureStore) {
    if (data) {
      this.breadcrumbs.push({
        componentId: data.componentStructureId,
        componentName: data.componentStructure?.name,
        storeId: data.id,
        storeName: data.identifier,
      });
      this.filterComponentByParentMapping(data.componentStructureId);
      this.filterStructureByParentMapping(data.id);
      this.currentParentId = data.id;
    }
  }

  public onBreadcrumbClick(index: number) {
    if (this.breadcrumbs.length == index + 1) {
      return;
    }
    if (index > -1) {
      const data: ComponentBreadcrumb = this.breadcrumbs[index];
      this.filterComponentByParentMapping(data.componentId);
      this.filterStructureByParentMapping(data.storeId);
      this.breadcrumbs = this.breadcrumbs.splice(0, index + 1);
    } else {
      this.currentParentId = 0;
      this.breadcrumbs = [];
      this.filterComponentByParentMapping(0);
      this.filterStructureByParentMapping(0);
    }
  }

  public onChangeStoreIdentifierName(data: StructureStore): void {
    clearTimeout(this.onChangeTimeOut);
    this.onChangeTimeOut = setTimeout(() => {
      const payload = { ...data, properties: [], subStructure: [] };
      this.dataService.postData<StructureStore>(`${Modules.StructureStore}`, payload).then((response: StructureStore) => {
        if (response) {
          this.getStructuresStore();
        }
      });
    }, 1000);
  }

  public onRegenerateIndex() {
    this.storeVisibleListIndex.forEach((item, index) => { item.shortIndex = index + 1 });
    this.storeVisibleListIndex = this.storeVisibleListIndex.sort((a, b) => a.shortIndex - b.shortIndex);
  }

  public onGeneratorStore(event: any): void {
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
      moveItemInArray(this.storeVisibleListIndex, event.previousIndex, event.currentIndex);
      this.onRegenerateIndex();
      this.reindexStoreOnServer();
    } else {
      const componentStructure: ComponentStructure = event.previousContainer.data[event.previousIndex];
      this.generateStoreOnServer(componentStructure);
    }
  }

  public generateStoreOnServer(componentStructure: ComponentStructure) {
    this.dataService
      .postData<StructureStore>(`${Modules.StructureStore}`, {
        parentStructureId: this.currentParentId,
        componentStructureId: componentStructure.id,
        identifier: `${componentStructure.name}-${newRandom(10)}`,
      })
      .then((response: StructureStore) => {
        if (response) {
          this.getStructuresStore();
        }
      });
  }

  public reindexStoreOnServer() {
    this.dataService.post(`${Modules.StructureStore}${RouteExtension.Reindex}`, this.storeVisibleListIndex);
  }

  public onGenerateComponent(): void {
    this.componentGenerator.onGenerateNewComponent();
  }

  public onUpdateComponent(data: number): void {
    this.componentGenerator.onUpdateComponent(data);
  }

  public onUpdateProperties(data: StructureStore): void {
    this.propertyGenerator.onShow({ ...data, subStructureList: this.storeOriginalList.filter((d) => data.childMapping?.includes(d.id)) });
  }

  public removeComponentStructure(id: number): void {
    this.alertModal.openModel();
    const alertSub$ = this.alertModal.onConfirm.subscribe((resp) => {
      this.dataService.delete(`${Modules.ComponentStructure}`, id).then((resp: any) => {
        if (resp) {
          this.getGeneratorData();
        }
      });
      if (alertSub$) {
        alertSub$.unsubscribe();
      }
    });
  }

  public removeStructureStore(id: number): void {
    this.alertModal.openModel();
    const alertSub$ = this.alertModal.onConfirm.subscribe((resp) => {
      this.dataService.delete(`${Modules.StructureStore}`, id).then((resp: any) => {
        if (resp) {
          this.getGeneratorData();
        }
      });
      if (alertSub$) {
        alertSub$.unsubscribe();
      }
    });
  }

  public onClosePopup(refresh: boolean): void {
    if (refresh) {
      this.getGeneratorData();
    }
  }
}
