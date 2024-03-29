import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule} from '@angular/router';
import { HomeComponent } from './Components/home/home.component';

const appRoutes: Routes = [
    { path: '', component: HomeComponent }
];

export const appRoutningProviders: any[] = [];
export const routing: ModuleWithProviders = RouterModule.forRoot(appRoutes);