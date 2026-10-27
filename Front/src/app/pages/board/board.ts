import { ChangeDetectionStrategy, Component, OnInit, computed, inject, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CdkDragDrop, DragDropModule } from '@angular/cdk/drag-drop';
import { StatusTarefa, Tarefa, TarefaFiltro, TarefaRequest } from '../../core/dtos/tarefa.dto';
import { TarefaService } from '../../core/services/tarefa.service';
import { COLUNAS } from '../../core/utils/tarefa.util';
import { TaskCard } from '../../shered/task-card/task-card';
import { TaskFormDialog } from '../../shered/task-form-dialog/task-form-dialog';

@Component({
  selector: 'app-board',
  imports: [FormsModule, DragDropModule, TaskCard, TaskFormDialog],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './board.html',
})
export class Board implements OnInit {
  private readonly service = inject(TarefaService);

  protected readonly colunas = COLUNAS;
  protected readonly tarefas = signal<Tarefa[]>([]);
  protected readonly carregando = signal(false);
  protected readonly mensagem = signal<string | null>(null);
  protected readonly erroForm = signal<string | null>(null);
  protected readonly dialogAberto = signal(false);
  protected readonly editando = signal<Tarefa | null>(null);

  protected filtroStatus: StatusTarefa | '' = '';
  protected filtroDe = '';
  protected filtroAte = '';

  protected readonly porColuna = computed(() => {
    const mapa = new Map<StatusTarefa, Tarefa[]>(COLUNAS.map((c) => [c.status, []]));
    for (const t of this.tarefas()) 
      mapa.get(t.status)?.push(t);
    return mapa;
  });

  ngOnInit(): void {
    this.carregar();
  }

  protected carregar(): void {
    const filtro: TarefaFiltro = {
      status: this.filtroStatus,
      vencimentoDe: this.filtroDe,
      vencimentoAte: this.filtroAte,
    };
    this.carregando.set(true);
    this.service.listar(filtro).subscribe({
      next: (lista) => {
        this.tarefas.set(lista);
        this.carregando.set(false);
      },
      error: () => {
        this.carregando.set(false);
        this.mensagem.set('Não foi possível carregar as tarefas. Verifique se a API está em execução.');
      },
    });
  }

  protected limparFiltros(): void {
    this.filtroStatus = '';
    this.filtroDe = '';
    this.filtroAte = '';
    this.carregar();
  }

  protected soltar(event: CdkDragDrop<StatusTarefa>): void {
    if (event.previousContainer === event.container) return;
    const tarefa: Tarefa = event.item.data;
    this.moverPara(tarefa, event.container.data);
  }

  protected moverPara(tarefa: Tarefa, status: StatusTarefa): void {
    if (tarefa.status === status) 
      return;

    const anterior = tarefa.status;
    this.mensagem.set(null);
    this.substituir({ ...tarefa, status });

    this.service.atualizarStatus(tarefa.id, status).subscribe({
      next: (atual) => this.substituir(atual),
      error: () => {
        this.substituir({ ...tarefa, status: anterior });
        this.mensagem.set('Não foi possível alterar o status da tarefa.');
      },
    });
  }

  protected novaTarefa(): void {
    this.editando.set(null);
    this.erroForm.set(null);
    this.dialogAberto.set(true);
  }

  protected editar(tarefa: Tarefa): void {
    this.editando.set(tarefa);
    this.erroForm.set(null);
    this.dialogAberto.set(true);
  }

  protected fecharDialog(): void {
    this.dialogAberto.set(false);
  }

  protected salvar(dto: TarefaRequest): void {
    const atual = this.editando();
    const req = atual ? this.service.atualizar(atual.id, dto) : this.service.criar(dto);

    req.subscribe({
      next: () => {
        this.dialogAberto.set(false);
        this.carregar();
      },
      error: (e: HttpErrorResponse) => this.erroForm.set(e.error?.title ?? 'Erro ao salvar a tarefa.'),
    });
  }

  protected excluir(tarefa: Tarefa): void {
    if (!confirm(`Excluir a tarefa "${tarefa.titulo}"?`)) 
      return;

    this.service.remover(tarefa.id).subscribe({
      next: () => this.tarefas.update((lista) => lista.filter((t) => t.id !== tarefa.id)),
      error: () => this.mensagem.set('Não foi possível excluir a tarefa.'),
    });
  }

  private substituir(tarefa: Tarefa): void {
    this.tarefas.update((lista) => lista.map((t) => (t.id === tarefa.id ? tarefa : t)));
  }
}
