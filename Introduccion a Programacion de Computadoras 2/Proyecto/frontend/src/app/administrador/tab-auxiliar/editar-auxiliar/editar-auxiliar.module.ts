import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Routes, RouterModule } from '@angular/router';

import { IonicModule } from '@ionic/angular';

import { EditarAuxiliarPage } from './editar-auxiliar.page';

const routes: Routes = [
  {
    path: '',
    component: EditarAuxiliarPage
  }
];

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    RouterModule.forChild(routes)
  ],
  declarations: [EditarAuxiliarPage]
})
export class EditarAuxiliarPageModule {}
