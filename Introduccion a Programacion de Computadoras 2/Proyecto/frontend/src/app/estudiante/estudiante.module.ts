import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Routes, RouterModule } from '@angular/router';

import { IonicModule } from '@ionic/angular';

import { EstudiantePage } from './estudiante.page';

const routes: Routes = [
  {
    path: '',
    component: EstudiantePage,
    children: [
      {
        path: 'contacts',
        children: [
          {
            path: '',
            loadChildren: () => import('./tab1/tab1.module').then(m => m.Tab1PageModule)
          }
        ]
      },
      {
        path: 'mensajes',
        children: [
          {
            path: '',
            loadChildren: () => import('./tab2/tab2.module').then(m => m.Tab2PageModule)
          }
        ]
      },
      {
        path: 'evaluaciones',
        children: [
          {
            path: '',
            loadChildren: () => import('./tab3/tab3.module').then(m => m.Tab3PageModule)
          }
        ]
      },
      {
        path: 'actividades',
        children: [
          {
            path: '',
            loadChildren: () => import('./tab4/tab4.module').then(m => m.Tab4PageModule)
          }
        ]
      },
      {
        path: '',
        redirectTo: '/estudiante/contacts',
        pathMatch: 'full',
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
  declarations: [EstudiantePage]
})
export class EstudiantePageModule {}
