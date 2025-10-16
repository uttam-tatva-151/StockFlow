
import { Routes } from "@angular/router";
import { RoutePath } from "../../shared/constants";
import { AuthLayoutComponent } from "./authLayout.component";
import { LoginComponent, RegisterComponent } from "../../pages/AuthModule";

export  const authLayoutRoutes: Routes = [
  {
    path:'',
    component: AuthLayoutComponent,
    children: [
      {
        path: '',
        redirectTo: `${RoutePath.AUTH_LAYOUT.LOGIN_PATH}`,
        pathMatch: 'full'
      },
      {
        path: `${RoutePath.AUTH_LAYOUT.LOGIN_PATH}`,
        component: LoginComponent
      },
      {
        path: `${RoutePath.AUTH_LAYOUT.REGISTER_PATH}`,
        component: RegisterComponent
      }
    ]
  },

];