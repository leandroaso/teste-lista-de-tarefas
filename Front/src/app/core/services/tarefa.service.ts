import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { StatusTarefa, Tarefa, TarefaFiltro, TarefaRequest } from '../dtos/tarefa.dto';

@Injectable({ providedIn: 'root' })
export class TarefaService {
  private readonly http = inject(HttpClient);
  private readonly url = `${environment.apiBase}/tarefas`;

  listar(filtro: TarefaFiltro = {}): Observable<Tarefa[]> {
    let params = new HttpParams();

    if (filtro.status) 
      params = params.set('status', filtro.status);

    if (filtro.vencimentoDe) 
      params = params.set('vencimentoDe', filtro.vencimentoDe);

    if (filtro.vencimentoAte) 
      params = params.set('vencimentoAte', filtro.vencimentoAte);
    
    return this.http.get<Tarefa[]>(this.url, { params });
  }

  criar(dto: TarefaRequest): Observable<Tarefa> {
    return this.http.post<Tarefa>(this.url, dto);
  }

  atualizar(id: number, dto: TarefaRequest): Observable<Tarefa> {
    return this.http.put<Tarefa>(`${this.url}/${id}`, dto);
  }

  atualizarStatus(id: number, status: StatusTarefa): Observable<Tarefa> {
    return this.http.patch<Tarefa>(`${this.url}/${id}/status`, { status });
  }

  remover(id: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/${id}`);
  }
}
