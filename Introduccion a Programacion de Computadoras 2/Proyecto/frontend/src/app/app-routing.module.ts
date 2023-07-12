import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: 'home', loadChildren: () => import('./home/home.module').then( m => m.HomePageModule)},
  { path: 'estudiante', loadChildren: './estudiante/estudiante.module#EstudiantePageModule' },
  { path: 'auxiliar', loadChildren: './auxiliar/auxiliar.module#AuxiliarPageModule' },
  { path: 'administrador', loadChildren: './administrador/administrador.module#AdministradorPageModule' },
  { path: 'tab1', loadChildren: './estudiante/tab1/tab1.module#Tab1PageModule' },
  { path: 'tab2', loadChildren: './estudiante/tab2/tab2.module#Tab2PageModule' },
  { path: 'tab3', loadChildren: './estudiante/tab3/tab3.module#Tab3PageModule' },
  { path: 'tab4', loadChildren: './estudiante/tab4/tab4.module#Tab4PageModule' },
  { path: 'estudiante_select', loadChildren: './estudiante-select/estudiante-select.module#EstudianteSelectPageModule' },
  { path: 'tab-std1', loadChildren: './estudiante-select/tab-std1/tab-std1.module#TabStd1PageModule' },
  { path: 'tab-std2', loadChildren: './estudiante-select/tab-std2/tab-std2.module#TabStd2PageModule' },
  { path: 'tab-foro', loadChildren: './auxiliar/tab-foro/tab-foro.module#TabForoPageModule' },
  { path: 'tab-mensajes', loadChildren: './auxiliar/tab-mensajes/tab-mensajes.module#TabMensajesPageModule' },
  { path: 'tab-evaluaciones', loadChildren: './auxiliar/tab-evaluaciones/tab-evaluaciones.module#TabEvaluacionesPageModule' },
  { path: 'tab-actividades', loadChildren: './auxiliar/tab-actividades/tab-actividades.module#TabActividadesPageModule' },
  { path: 'administrador/asignaciones', loadChildren: './administrador/tab-asignar/tab-asignar.module#TabAsignarPageModule' },
  { path: 'administrador/auxiliares', loadChildren: './administrador/tab-auxiliar/tab-auxiliar.module#TabAuxiliarPageModule' },
  { path: 'administrador/cursos', loadChildren: './administrador/tab-cursos/tab-cursos.module#TabCursosPageModule' },
  { path: 'crear-curso', loadChildren: './administrador/tab-cursos/crear-curso/crear-curso.module#CrearCursoPageModule' },
  { path: 'editar-curso', loadChildren: './administrador/tab-cursos/editar-curso/editar-curso.module#EditarCursoPageModule' },
  { path: 'crear-auxiliar', loadChildren: './administrador/tab-auxiliar/crear-auxiliar/crear-auxiliar.module#CrearAuxiliarPageModule' },
  { path: 'editar-auxiliar', loadChildren: './administrador/tab-auxiliar/editar-auxiliar/editar-auxiliar.module#EditarAuxiliarPageModule' },
  { path: 'asignar-auxiliar', loadChildren: './administrador/tab-asignar/asignar-auxiliar/asignar-auxiliar.module#AsignarAuxiliarPageModule' },
  { path: 'asignar-curso', loadChildren: './administrador/tab-asignar/asignar-curso/asignar-curso.module#AsignarCursoPageModule' },
  { path: 'administrador/consultas', loadChildren: './administrador/tab-consultas/tab-consultas.module#TabConsultasPageModule' },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
