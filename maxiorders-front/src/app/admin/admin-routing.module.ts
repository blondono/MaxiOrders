import { NgModule } from "@angular/core";
import { RouterModule, Routes, Router } from '@angular/router';

import { MainComponent } from './components/main/main.component';
import { AdminGuard } from './services/admin.guard';
import { LoginComponent } from './components/login/login.component';
import { EquiposComponent } from './components/equipos/equipos.component';
import { ClienteComponent } from './components/Clientes/clientes.component';
import { SedesComponent } from './components/sedes/sedes.component';

const adminRoutes: Routes = [
    { path: 'admin/login', component: LoginComponent },
    {
        path: 'admin',
        component: MainComponent,
        canActivate: [AdminGuard],
        children: [
            { path: '', redirectTo: 'admin', pathMatch: 'full' },
            { path: 'equipos', component: EquiposComponent },
            { path: 'clientes', component: ClienteComponent },
            { path: 'sedes', component: SedesComponent }
        ]
    }
]

@NgModule({
    imports: [
        RouterModule.forChild(adminRoutes)
    ],
    exports: [
        RouterModule
    ]
})
export class AdminRoutingModule { }