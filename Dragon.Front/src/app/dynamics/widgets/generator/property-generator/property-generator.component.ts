import { Component, ViewChild } from '@angular/core';
import { DataService } from '@services/data.service';
import { StructureStore, StructureStoreProperty } from '@services/model/dynamics/generator.model';
import { Modules } from '@urls';
import { ModalComponent } from 'angular-custom-modal';

@Component({
  selector: 'app-property-generator',
  templateUrl: './property-generator.component.html',
  styleUrls: ['./property-generator.component.css']
})
export class PropertyGeneratorComponent {
  public editStructure!: StructureStore;
  public editProperty!: StructureStoreProperty | null;
  public propertyTypeList: Array<string> = ['Boolean', 'String', 'Component'];

  @ViewChild('template', { static: true }) modelPropertyReference: ModalComponent;
  constructor(private dataService: DataService) { }

  onShow(data: StructureStore) {
    this.modelPropertyReference.open();
    this.processProperties(data);
  }

  public processProperties(data: StructureStore): void {
    if (data && data.properties) {
      data.properties.forEach((property: StructureStoreProperty) => {
        property.structureMappingNameList = data.subStructureList?.filter((d: StructureStore) => property.childMapping?.includes(d.id)).map(d => d.identifier).join(' | ');
        if (!property.componentStructureProperty?.isMultiple && property.childMapping) { property.childMapping = property.childMapping[0]; }
      });
    }
    this.editStructure = data;
  }

  public onUpdateProperty(): void {
    this.dataService.postData(Modules.StructureStoreProperty, {
      ...this.editProperty,
      componentStructureProperty: null,
      childMapping: this.editProperty?.componentStructureProperty?.isMultiple ? this.editProperty.childMapping : this.editProperty?.childMapping ? [this.editProperty?.childMapping] : []
    }).then((response: any) => {
      if (response) {
        this.editProperty = null;
        const propIndex = this.editStructure.properties.findIndex((d: StructureStoreProperty) => d.id === response.id);
        if (propIndex > -1) {
          this.editStructure.properties[propIndex].type = response.type;
          this.editStructure.properties[propIndex].childMapping = response.childMapping;
          this.processProperties(this.editStructure);
        }
      }
    });
  }

  public onChangePropertyType(): void {
    if (this.editProperty) {
      this.editProperty.value = '-';
      this.editProperty.childMapping = null;
    }
  }
}
