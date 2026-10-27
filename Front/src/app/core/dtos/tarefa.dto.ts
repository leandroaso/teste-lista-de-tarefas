export type StatusTarefa = 'Pendente' | 'EmAndamento' | 'Concluido';

export interface Tarefa {
  id: number;
  titulo: string;
  descricao: string | null;
  status: StatusTarefa;
  dataVencimento: string;
  criadoEm: string;
  atualizadoEm: string | null;
}

export interface TarefaRequest {
  titulo: string;
  descricao: string | null;
  dataVencimento: string;
  status: StatusTarefa;
}

export interface TarefaFiltro {
  status?: StatusTarefa | '';
  vencimentoDe?: string;
  vencimentoAte?: string;
}
