import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Routes, RouterModule } from '@angular/router';

import { IonicModule } from '@ionic/angular';

import { TabCursosPage } from './tab-cursos.page';

const routes: Routes = [
  {
    path: '',
    component: TabCursosPage,
    children: [
      {
        path: 'crear',
        children: [
          {
            path: '',
            loadChildren: () => import('./crear-curso/crear-curso.module').then(m => m.CrearCursoPageModule)
          }
        ]
      },
      {
        path: 'editar',
        children: [
          {
            path: '',
            loadChildren: () => import('./editar-curso/editar-curso.module').then(m => m.EditarCursoPageModule)
          }
        ]
      },
      {
        path: '',
        children: [
          {
            path: '',
            loadChildren: () => import('./crear-curso/crear-curso.module').then(m => m.CrearCursoPageModule)
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
  declarations: [TabCursosPage]
})
export class TabCursosPageModule {}
