import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Routes, RouterModule } from '@angular/router';

import { IonicModule } from '@ionic/angular';

import { EstudianteSelectPage } from './estudiante-select.page';

const routes: Routes = [
  {
    path: '',
    component: EstudianteSelectPage,
    children: [
      {
        path: 'asignarCurso',
        children: [
          {
            path: '',
            loadChildren: () => import('./tab-std1/tab-std1.module').then(m => m.TabStd1PageModule)
          }
        ] 
      },
      {
        path: 'entrarCurso',
        children: [
          {
            path: '',
            loadChildren: () => import('./tab-std2/tab-std2.module').then(m => m.TabStd2PageModule)
          }
        ]
      },
      {
        path: 'tickets',
        children: [
          {
            path:'',
            loadChildren: () => import('./tab1/tab1.module').then(m => m.Tab1PageModule)
          }
        ]
      },
      {
        path: '',
        redirectTo: '/estudiante_select/entrarCurso',
        pathMatch: 'full'
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
  declarations: [EstudianteSelectPage]
})
export class EstudianteSelectPageModule {}
