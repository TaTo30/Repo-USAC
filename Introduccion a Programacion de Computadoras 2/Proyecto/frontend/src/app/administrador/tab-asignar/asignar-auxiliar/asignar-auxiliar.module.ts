import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Routes, RouterModule } from '@angular/router';

import { IonicModule } from '@ionic/angular';

import { AsignarAuxiliarPage } from './asignar-auxiliar.page';

const routes: Routes = [
  {
    path: '',
    component: AsignarAuxiliarPage
  }
];

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    RouterModule.forChild(routes)
  ],
  declarations: [AsignarAuxiliarPage]
})
export class AsignarAuxiliarPageModule {}
