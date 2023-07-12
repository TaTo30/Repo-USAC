import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Routes, RouterModule } from '@angular/router';

import { IonicModule } from '@ionic/angular';

import { CrearAuxiliarPage } from './crear-auxiliar.page';

const routes: Routes = [
  {
    path: '',
    component: CrearAuxiliarPage
  }
];

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    RouterModule.forChild(routes)
  ],
  declarations: [CrearAuxiliarPage]
})
export class CrearAuxiliarPageModule {}
