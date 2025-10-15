import { Routes } from '@angular/router';
import { authLayoutRoutes } from '../layout/authLayout/auth-layout-routes';
import { RoutePath } from '../shared/constants';

export const routes: Routes = [
  {
    path: '',
    redirectTo: `${RoutePath.WELCOME.PATH}`,
    pathMatch: 'full'
  },
  {
    path: `${RoutePath.WELCOME.PATH}`,
    children: authLayoutRoutes
  }
];


