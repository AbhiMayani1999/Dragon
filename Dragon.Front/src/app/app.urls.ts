import { environment } from 'src/environments/environment';

export const FixedRoutes = {
  Root: '/',
  Auth: 'auth',
  Dashboard: 'dashboard',
};

export const Modules = {
  Base: environment.apiURL,
  FileUrl: environment.fileURL,
  Auth: `${environment.apiURL}api/Auth`,
  Config: { MyNavigations: `${environment.apiURL}api/config/mynavigations` },
  Domain: { GetPage: `${environment.apiURL}api/domain/getpage` },
  ComponentStructure: `${environment.apiURL}api/componentstructure`,
  ComponentProperty: `${environment.apiURL}api/componentproperty`,
  StructureStore: `${environment.apiURL}api/structure`,
  StructureStoreProperty: `${environment.apiURL}api/structureproperty`,
};

export const RouteExtension = {
  Reindex: `/reindex`
};
