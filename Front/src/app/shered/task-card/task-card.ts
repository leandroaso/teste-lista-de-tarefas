import { ChangeDetectionStrategy, Component, computed, input, output } from '@angular/core';
import { DatePipe } from '@angular/common';
import { StatusTarefa, Tarefa } from '../../core/dtos/tarefa.dto';
import { COLUNAS, estaVencida } from '../../core/utils/tarefa.util';

@Component({
  selector: 'app-task-card',
  imports: [DatePipe],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './task-card.html',
})
export class TaskCard {
  readonly tarefa = input.required<Tarefa>();
  readonly statusChange = output<StatusTarefa>();
  readonly editar = output<Tarefa>();
  readonly excluir = output<Tarefa>();

  protected readonly colunas = COLUNAS;
  protected readonly vencida = computed(() => estaVencida(this.tarefa().dataVencimento, this.tarefa().status));

  protected alterarStatus(event: Event): void {
    const novo = (event.target as HTMLSelectElement).value as StatusTarefa;
    
    if (novo !== this.tarefa().status) 
      this.statusChange.emit(novo);
  }
}
