import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Routes, RouterModule } from '@angular/router';

import { IonicModule } from '@ionic/angular';

import { AuxiliarPage } from './auxiliar.page';

const routes: Routes = [
  {
    path: '',
    component: AuxiliarPage,
    children: [
      {
        path: 'foro',
        children: [
        {
          path: '',
          loadChildren: () => import('./tab-foro/tab-foro.module').then(m => m.TabForoPageModule)
        }
        ]
      },
      {
        path: 'mensajes',
        children: [
        {
          path: '',
          loadChildren: () => import('./tab-mensajes/tab-mensajes.module').then(m => m.TabMensajesPageModule)
        }
        ]
      },
      {
        path: 'evaluaciones',
        children: [
        {
          path: '',
          loadChildren: () => import('./tab-evaluaciones/tab-evaluaciones.module').then(m => m.TabEvaluacionesPageModule)
        }
        ]
      },
      {
        path: 'actividades',
        children: [
        {
          path: '',
          loadChildren: () => import('./tab-actividades/tab-actividades.module').then(m => m.TabActividadesPageModule)
        }
        ]
      },
      /*{
        path: '',
        redirectTo: '/auxiliar/foro',
        pathMatch: 'full',
      }*/
    ]
  }
];

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    RouterModule.forChild(routes)
  ],
  declarations: [AuxiliarPage]
})
export class AuxiliarPageModule {}
