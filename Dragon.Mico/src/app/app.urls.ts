import { environment } from "src/environments/environment";

export const FixedRoutes = {
  Root: '',
  Auth: 'auth',
  Dashboard: 'dashboard',
};

export const Modules = {
  Base: environment.apiURL,
  FileUrl: environment.fileURL,
  Images: {
    Temp: `${environment.apiURL}dragon/zzTemp`,
    Tenant: `${environment.apiURL}dragon`
  },
  Files: {
    Tempfile: `${environment.apiURL}file/temp`,
    Tenantfile: `${environment.apiURL}file/tenant`
  },
  Auth: `${environment.apiURL}api/auth`,
  Option: `${environment.apiURL}api/option`,
  Config: {
    MyNavigations: `${environment.apiURL}api/config/mynavigations`
  },
  Navigation:`${environment.apiURL}api/Navigation`,
  UserTypeNavigation:`${environment.apiURL}api/UserTypeNavigation`,
  UserNavigation:`${environment.apiURL}api/UserNavigation`
};

export const RouteExtensions = {
  Download: 'download',
  GetPage: 'getpage',
  Image: 'images'
}
