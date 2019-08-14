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


@NgModule({
    declarations: [
        MainComponent,
        LoginComponent
    ],
    imports: [
        CommonModule,
        FormsModule,
        HttpModule,
        AdminRoutingModule
    ],
    exports: [
        MainComponent,
        LoginComponent
    ],
    providers: [
        UserService,
        AdminGuard
    ]
})

export class AdminModule { }