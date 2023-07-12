import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from "./login/login.component";
import { RegistroComponent } from "./registro/registro.component";
import { PerfilComponent } from "./perfil/perfil.component";
import { AdministradorComponent } from "./administrador/administrador.component";
import { PublicacionesUserComponent } from "./publicaciones-user/publicaciones-user.component";
import { PublicacionesHomeComponent } from "./publicaciones-home/publicaciones-home.component";
import { CarritoComponent } from "./carrito/carrito.component";

const routes: Routes = [
  {path: '', redirectTo: '/home', pathMatch: 'full'},
  {path:'login', component: LoginComponent},
  {path:'registro', component: RegistroComponent},
  {path:'perfil', component: PerfilComponent},
  {path:'administrador', component: AdministradorComponent},
  {path:'mis_publicaciones', component: PublicacionesUserComponent},
  {path:'home', component: PublicacionesHomeComponent},
  {path:'mis_compras', component: CarritoComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
