import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/root/app.component';
import { appConfig } from './app/root/app.config';


bootstrapApplication(AppComponent, appConfig).catch((err) => console.error(err));
