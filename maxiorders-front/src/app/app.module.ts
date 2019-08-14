import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
//Plugins
import { EditorModule } from '@tinymce/tinymce-angular';

//modulos
import { routing, appRoutningProviders} from './app.routing';


import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

// Componentes

import { AdminModule } from './admin/admin.module';
import { HomeComponent } from './Components/home/home.component';


//Servicios

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    FormsModule,
    EditorModule,
    AdminModule,
    routing
  ],
  providers: [
    appRoutningProviders
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
