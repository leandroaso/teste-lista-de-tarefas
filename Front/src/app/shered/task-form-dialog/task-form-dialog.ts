import { ChangeDetectionStrategy, Component, OnInit, inject, input, output } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Tarefa, TarefaRequest } from '../../core/dtos/tarefa.dto';
import { COLUNAS, dataParaApi, dataParaInput } from '../../core/utils/tarefa.util';

@Component({
  selector: 'app-task-form-dialog',
  imports: [ReactiveFormsModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './task-form-dialog.html',
})
export class TaskFormDialog implements OnInit {
  private readonly formBuilder = inject(FormBuilder).nonNullable;

  readonly tarefa = input<Tarefa | null>(null);
  readonly erro = input<string | null>(null);
  readonly salvou = output<TarefaRequest>();
  readonly cancelar = output<void>();

  protected readonly colunas = COLUNAS;
  protected readonly form = this.formBuilder.group({
    titulo: ['', [Validators.required, Validators.maxLength(200)]],
    descricao: ['', Validators.maxLength(1000)],
    dataVencimento: ['', Validators.required],
    status: ['Pendente' as TarefaRequest['status']],
  });

  ngOnInit(): void {
    const novaTarefa = this.tarefa();
    if (novaTarefa) {
      this.form.setValue({
        titulo: novaTarefa.titulo,
        descricao: novaTarefa.descricao ?? '',
        dataVencimento: dataParaInput(novaTarefa.dataVencimento),
        status: novaTarefa.status,
      });
    }
  }

  protected salvar(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    const formValues = this.form.getRawValue();
    this.salvou.emit({
      titulo: formValues.titulo.trim(),
      descricao: formValues.descricao.trim() || null,
      dataVencimento: dataParaApi(formValues.dataVencimento),
      status: formValues.status,
    });
  }
}
