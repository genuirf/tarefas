using shared.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using tarefas.API;
using tarefas.Controls;
using tarefas.Model;
using util;

namespace tarefas.ViewModel
{
      public class TarefaViewModel : ViewModel
      {
            public TarefaViewModel()
            {
                  _tarefa = new();
            }

            private TarefaGrupo _grupoParent;
            public TarefaGrupo GrupoParent
            {
                  get => _grupoParent;
                  set
                  {
                        _grupoParent = value;
                        OnPropertyChanged();
                  }
            }

            private Tarefa _tarefa;
            public Tarefa tarefa
            {
                  get => _tarefa;
                  set
                  {
                        _tarefa = value;
                        OnPropertyChanged();
                  }
            }

            public event EventHandler<EventArgs>? CloseEvent;

            private ICommand _saveCommand;
            public ICommand SaveCommand => _saveCommand ??= new AsyncCommand<object?>(async (parameter) =>
            {
                  await SalvarAsync(parameter);

            }, (parameter) => true); 

            private ICommand _cancelCommand;
            public ICommand CancelCommand => _cancelCommand ??= new AsyncCommand<object?>(async (parameter) =>
            {
                  CloseEvent?.Invoke(this, EventArgs.Empty);

            }, (parameter) => true); 


            private async Task  SalvarAsync(object? arg)
            {

                  if (tarefa.IsNew)
                  {
                        await Api.AddTarefa(tarefa)                        
                            .ContinueWith((task) =>
                            {
                                  if (task.Status == TaskStatus.RanToCompletion)
                                  {
                                        tarefa.Id = task.Result.tarefa.Id; // resgatar Id inserido no registro
                                        tarefa.Titulo = task.Result.tarefa.Titulo;
                                        tarefa.Descricao = task.Result.tarefa.Descricao;
                                        tarefa.ordem = task.Result.tarefa.ordem;
                                        tarefa.DataCadastro = task.Result.tarefa.DataCadastro;
                                        tarefa.DataConclusao = task.Result.tarefa.DataConclusao;
                                        tarefa.concluido = task.Result.tarefa.concluido;
                                        tarefa.arquivado = task.Result.tarefa.arquivado;

                                        GrupoParent.tarefas.Add(tarefa);

                                        CloseEvent?.Invoke(this, EventArgs.Empty);

                                  }
                                  else if (task.IsFaulted)
                                  {
                                        // TODO tratar erro aqui

                                  }
                            });
                  }
                  else
                  {
                        await Api.UpdateTarefa(tarefa)
                            .ContinueWith((task) =>
                            {
                                  if (task.Status == TaskStatus.RanToCompletion)
                                  {
                                        tarefa.Titulo = task.Result.tarefa.Titulo;
                                        tarefa.Descricao = task.Result.tarefa.Descricao;
                                        tarefa.ordem = task.Result.tarefa.ordem;
                                        tarefa.DataCadastro = task.Result.tarefa.DataCadastro;
                                        tarefa.DataConclusao = task.Result.tarefa.DataConclusao;
                                        tarefa.concluido = task.Result.tarefa.concluido;
                                        tarefa.arquivado = task.Result.tarefa.arquivado;

                                        CloseEvent?.Invoke(this, EventArgs.Empty);
                                  }
                                  else if (task.IsFaulted)
                                  {
                                        // TODO tratar erro aqui

                                  }
                            });
                  }
            }

      }
}
