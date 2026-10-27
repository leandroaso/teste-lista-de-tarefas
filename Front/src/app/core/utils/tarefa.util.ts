import { StatusTarefa } from '../dtos/tarefa.dto';

export interface ColunaInfo {
  status: StatusTarefa;
  titulo: string;
  corBorda: string;
  corBadge: string;
}

export const COLUNAS: readonly ColunaInfo[] = [
  { status: 'Pendente', titulo: 'Pendente', corBorda: 'border-amber-400', corBadge: 'bg-amber-100 text-amber-800' },
  { status: 'EmAndamento', titulo: 'Em andamento', corBorda: 'border-sky-400', corBadge: 'bg-sky-100 text-sky-800' },
  { status: 'Concluido', titulo: 'Concluído', corBorda: 'border-emerald-400', corBadge: 'bg-emerald-100 text-emerald-800' },
];

export function tituloStatus(status: StatusTarefa): string {
  return COLUNAS.find((c) => c.status === status)?.titulo ?? status;
}

/** Converte "yyyy-MM-dd" do input date para o formato enviado à API. */
export function dataParaApi(data: string): string {
  return `${data}T00:00:00`;
}

export function dataParaInput(dataApi: string): string {
  return dataApi.slice(0, 10);
}

export function estaVencida(dataApi: string, status: StatusTarefa): boolean {
  if (status === 'Concluido') return false;
  const hoje = new Date();
  const hojeStr = `${hoje.getFullYear()}-${String(hoje.getMonth() + 1).padStart(2, '0')}-${String(hoje.getDate()).padStart(2, '0')}`;
  return dataParaInput(dataApi) < hojeStr;
}
