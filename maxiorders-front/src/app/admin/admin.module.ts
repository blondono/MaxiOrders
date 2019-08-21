//Importación de modulos
import { NgModule } from "@angular/core";
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AdminRoutingModule } from './admin-routing.module';
import { HttpModule } from '@angular/http';

import { MainComponent } from './components/main/main.component';
import { LoginComponent } from './components/login/login.component';
import { AdminGuard } from './services/admin.guard';
import { UserService } from '../services/user.service';
import { EquiposComponent } from './components/equipos/equipos.component';
import { BaseService } from './services/base.master';
import { SearchPipe } from '../pipes/search.pipe';

@NgModule({
    declarations: [
        MainComponent,
        LoginComponent,
        EquiposComponent,
        SearchPipe
    ],
    imports: [
        CommonModule,
        FormsModule,
        AdminRoutingModule,
        HttpModule
    ],
    exports: [
        MainComponent,
        LoginComponent,
        EquiposComponent
    ],
    providers: [
        UserService,
        BaseService,
        AdminGuard
    ]
})

export class AdminModule { }