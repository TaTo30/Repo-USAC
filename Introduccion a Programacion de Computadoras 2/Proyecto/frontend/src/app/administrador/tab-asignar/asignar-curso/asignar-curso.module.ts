import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Routes, RouterModule } from '@angular/router';

import { IonicModule } from '@ionic/angular';

import { AsignarCursoPage } from './asignar-curso.page';

const routes: Routes = [
  {
    path: '',
    component: AsignarCursoPage
  }
];

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    RouterModule.forChild(routes)
  ],
  declarations: [AsignarCursoPage]
})
export class AsignarCursoPageModule {}
