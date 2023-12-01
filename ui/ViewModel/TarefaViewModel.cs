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

            private Tarefa _original;
            public Tarefa original
            {
                  get => _original;
                  set
                  {
                        _original = value;
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
                  if (tarefa.concluido && !tarefa.DataConclusao.HasValue)
                  {
                        tarefa.DataConclusao = DateTime.Now;
                  }
                  else if (!tarefa.concluido)
                  {
                        tarefa.DataConclusao = null;
                  }

                  if (tarefa.IsNew)
                  {
                        await Api.AddTarefa(tarefa)                        
                            .ContinueWith((task) =>
                            {
                                  if (task.Status == TaskStatus.RanToCompletion)
                                  {
                                        original.Id = task.Result.tarefa.Id; // resgatar Id inserido no registro
                                        original.grupo_Id = task.Result.tarefa.grupo_Id;
                                        original.Titulo = task.Result.tarefa.Titulo;
                                        original.Descricao = task.Result.tarefa.Descricao;
                                        original.ordem = task.Result.tarefa.ordem;
                                        original.DataCadastro = task.Result.tarefa.DataCadastro;
                                        original.DataConclusao = task.Result.tarefa.DataConclusao;
                                        original.concluido = task.Result.tarefa.concluido;
                                        original.arquivado = task.Result.tarefa.arquivado;

                                        App.Current.Dispatcher.Invoke(() => GrupoParent.tarefas.Add(original));

                                        App.Current.Dispatcher.Invoke(() => CloseEvent?.Invoke(this, EventArgs.Empty));

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
                                        original.grupo_Id = task.Result.tarefa.grupo_Id;
                                        original.Titulo = task.Result.tarefa.Titulo;
                                        original.Descricao = task.Result.tarefa.Descricao;
                                        original.ordem = task.Result.tarefa.ordem;
                                        original.DataCadastro = task.Result.tarefa.DataCadastro;
                                        original.DataConclusao = task.Result.tarefa.DataConclusao;
                                        original.concluido = task.Result.tarefa.concluido;
                                        original.arquivado = task.Result.tarefa.arquivado;

                                        App.Current.Dispatcher.Invoke(() => CloseEvent?.Invoke(this, EventArgs.Empty));
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
