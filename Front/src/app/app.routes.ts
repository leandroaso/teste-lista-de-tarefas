import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'tarefas' },
  { path: 'tarefas', loadComponent: () => import('./pages/board/board').then((m) => m.Board) },
  { path: '**', redirectTo: 'tarefas' },
];
