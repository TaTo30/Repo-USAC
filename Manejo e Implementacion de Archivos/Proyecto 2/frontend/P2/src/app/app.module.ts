import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from "@angular/forms";
import { HttpClientModule } from "@angular/common/http";
import { DatePipe } from "@angular/common";

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { RegistroComponent } from './registro/registro.component';
import { PerfilComponent } from './perfil/perfil.component';
import { AdministradorComponent } from './administrador/administrador.component';
import { PublicacionesUserComponent } from './publicaciones-user/publicaciones-user.component';
import { PublicacionesHomeComponent } from './publicaciones-home/publicaciones-home.component';
import { CarritoComponent } from './carrito/carrito.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegistroComponent,
    PerfilComponent,
    AdministradorComponent,
    PublicacionesUserComponent,
    PublicacionesHomeComponent,
    CarritoComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule
  ],
  providers: [
    HttpClientModule,
    DatePipe
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
