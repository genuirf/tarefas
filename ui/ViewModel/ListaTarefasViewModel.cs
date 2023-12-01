using shared.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using tarefas.API;
using tarefas.Model;

namespace tarefas.ViewModel
{
      public class ListaTarefasViewModel : ViewModel
      {
            public ListaTarefasViewModel()
            {

                  _tarefa_grupos = new();

#if DEBUG
                  if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
                  {
                        _tarefa_grupos.Add(new(_tarefa_grupos) { Descricao = "Prerequisitos",
                              tarefas = new ObservableCollection<Tarefa> {
                              new Tarefa { Titulo = "Tarefa 1", Descricao = "Prerequisitos 1", DataCadastro = DateTime.Now }
                        }
                        });
                        _tarefa_grupos.Add(new(_tarefa_grupos) { Descricao = "Desenvolvimento",
                              tarefas = new ObservableCollection<Tarefa> {
                              new Tarefa { Titulo = "Tarefa 1", Descricao = "Descrição Tarefa 1", DataCadastro = DateTime.Now },
                              new Tarefa { Titulo = "Tarefa 2", Descricao = "Descrição Tarefa 2" , DataCadastro = DateTime.Now},
                              new Tarefa { Titulo = "Tarefa 3", Descricao = "Descrição Tarefa 3" , DataCadastro = DateTime.Now},
                              new Tarefa { Titulo = "Tarefa 4", Descricao = "Descrição Tarefa 4" , DataCadastro = DateTime.Now}
                        }
                        });
                        _tarefa_grupos.Add(new(_tarefa_grupos) { Descricao = "Testes" });
                        _tarefa_grupos.Add(new(_tarefa_grupos) { Descricao = "Release" });
                  }
#endif

                  Api.ListGrupo()
                      .ContinueWith((task) =>
                      {
                            if (task.Status == TaskStatus.RanToCompletion)
                            {
                                  App.Current.Dispatcher.Invoke(() =>
                                  {
                                        foreach (var item in task.Result.grupos)
                                        {
                                              TarefaGrupo grupo = new(_tarefa_grupos) { Id = item.Id, ordem = item.ordem, Descricao = item.Descricao };
                                              _tarefa_grupos.Add(grupo);

                                              Api.ListTarefa(grupo.Id).ContinueWith((task) =>
                                              {
                                                    if (task.Status == TaskStatus.RanToCompletion)
                                                    {
                                                          App.Current.Dispatcher.Invoke(() =>
                                                          {
                                                                foreach (var item in task.Result.tarefas)
                                                                {
                                                                      grupo.tarefas.Add(item);
                                                                }
                                                          });
                                                    }
                                              });
                                        }
                                  });
                            }
                            else if (task.IsFaulted)
                            {
                                  // TODO tratar erro aqui

                            }
                      });
            }

            private ObservableCollection<TarefaGrupo> _tarefa_grupos;
            public ObservableCollection<TarefaGrupo> tarefa_grupos { get { return _tarefa_grupos; } set { _tarefa_grupos = value; } }
      }
}
