import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Routes, RouterModule } from '@angular/router';

import { IonicModule } from '@ionic/angular';

import { TabAsignarPage } from './tab-asignar.page';

const routes: Routes = [
  {
    path: '',
    component: TabAsignarPage,
    children: [
      {
        path: 'crear',
        children: [
          {
            path: '',
            loadChildren: () => import('./asignar-auxiliar/asignar-auxiliar.module').then(m => m.AsignarAuxiliarPageModule)
          }
        ]
      },
      {
        path: 'editar',
        children: [
          {
            path: '',
            loadChildren: () => import('./asignar-curso/asignar-curso.module').then(m => m.AsignarCursoPageModule)
          }
        ]
      },
      {
        path: '',
        children: [
          {
            path: '',
            loadChildren: () => import('./asignar-auxiliar/asignar-auxiliar.module').then(m => m.AsignarAuxiliarPageModule)
          }
        ]
      }
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
  declarations: [TabAsignarPage]
})
export class TabAsignarPageModule {}
