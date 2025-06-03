export interface ComponentBreadcrumb {
  componentId: number;
  componentName?: string;
  storeId: number;
  storeName?: string;
}

export interface ComponentProperty {
  id: number;
  name: string;
  isGeneric: boolean;
}

export interface ComponentStructure {
  id: number;
  name: string;
  isGeneric: boolean;
  canBeParent: boolean;
  properties: ComponentStructureProperty[];
  parentMapping?: number[];
  childMapping?: number[];
  propertyNameList?: string;
  subComponentNameList?: string;
}

export interface ComponentStructureProperty {
  id: number;
  isMultiple: boolean;
  componentStructureId: number;
  componentPropertyId: number;
  componentProperty: ComponentProperty | null;
  componentStructure: any;
}

export interface StructureStore {
  id: number;
  identifier: string;
  componentStructureId: number;
  componentStructure: ComponentStructure | null;
  parentStructureId?: number | null;
  parentMapping?: number[];
  childMapping?: number[];
  properties: StructureStoreProperty[];
  shortIndex?: number;

  identifierEnable?: boolean;
  propertyNameList?: string;
  subStructureNameList?: string;
  TitleProperty?: string;
  subStructureList?: StructureStore[] | null;
}

export interface StructureStoreProperty {
  id: number;
  componentStructurePropertyId: number;
  componentStructureProperty: ComponentStructureProperty | null;
  value: string;
  type: string;

  childMapping?: any;
  structureMappingNameList?: string;
}

export interface StructureStorePropertyToStructureStore {
  id: number;
  structureStoreId?: number;
  structureStore?: StructureStore;
  structureStorePropertyId?: number;
  structureStoreProperty?: StructureStoreProperty;
}

export interface StructureIndex{
  id: number;
  shortIndex: number;
}
