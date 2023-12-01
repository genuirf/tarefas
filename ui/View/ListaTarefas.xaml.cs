using shared.Model;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using tarefas.API;
using tarefas.Model;
using tarefas.ViewModel;

namespace tarefas.View
{
      /// <summary>
      /// Interaction logic for MainWindow.xaml
      /// </summary>
      public partial class ListaTarefas : Window
      {
            public ListaTarefasViewModel model;
            public ListaTarefas()
            {
                  InitializeComponent();

                  DataContext = model = new ListaTarefasViewModel();
            }

            private void BtAddGrupo_Click(object sender, RoutedEventArgs e)
            {

                  var grupo = new TarefaGrupo(model.tarefa_grupos);

                  var modal = new ModalAddGrupo();
                  modal.Show(container, "Adicionar Grupo", grupo, (g) => model.tarefa_grupos.Add(grupo));
            }

            private void TaskColumn_EditarGrupoEvent(object sender, Controls.EditarGrupoEventArgs e)
            {
                  var modal = new ModalAddGrupo();
                  modal.Show(container, "Editar Grupo", e.Grupo);
            }

            private void TaskColumn_ExcluirGrupoEvent(object sender, Controls.EditarGrupoEventArgs e)
            {
                  var modal = new ModalConfirma();
                  modal.Show(container, "Excluir este Grupo?", () =>
                  {
                        Api.DeleteGrupo(e.Grupo.Id)
                            .ContinueWith((task) =>
                            {
                                  if (task.Status == TaskStatus.RanToCompletion)
                                  {
                                        Dispatcher.Invoke(() =>
                                        {
                                              model.tarefa_grupos.Remove(e.Grupo);
                                        });
                                  }
                                  else if (task.IsFaulted)
                                  {
                                        // TODO tratar erro aqui

                                  }
                            });

                  });
            }

            private void TaskColumn_GrupoOrdenadoEvent(object sender, EventArgs e)
            {
                  List<Grupo> grupos = new();
                  foreach (var item in model.tarefa_grupos)
                  {
                        Grupo grupo = new();

                        grupo.Id = item.Id;
                        grupo.Descricao = item.Descricao;
                        grupo.ordem = grupos.Count;

                        grupos.Add(grupo);
                  }

                  Api.UpdateGrupoOrdem(grupos).ContinueWith((task) =>
                  {
                        if (task.Status == TaskStatus.RanToCompletion)
                        {
                             // TODO tratar sucesso aqui

                        }
                        else if (task.IsFaulted)
                        {
                              // TODO tratar erro aqui

                        }
                  });
            }

            private void TaskColumn_AddTarefaEvent(object sender, Controls.AddEditTarefaEventArgs e)
            {
                  Tarefa tarefa = new();
                  tarefa.DataCadastro = DateTime.Now;
                  tarefa.grupo_Id = e.Grupo.Id;

                  var modal = new ModalAddTarefa();
                  modal.Show(container, "Adicionar Tarefa", tarefa, e.Grupo);

            }

            private void TaskColumn_EditTarefaEvent(object sender, Controls.AddEditTarefaEventArgs e)
            {
                  var modal = new ModalAddTarefa();
                  modal.Show(container, "Editar Tarefa",  e.Tarefa!, e.Grupo);
            }

            private void TaskColumn_TarefaOrdenadaEvent(object sender, Controls.TarefaOrdenadaEventArgs e)
            {
                  List<Tarefa> tarefas = new();
                  foreach (var grupo in e.Grupos)
                  {
                        int index = 0;
                        foreach (var tarefa in grupo.tarefas)
                        {

                              tarefa.ordem = index;

                              tarefas.Add(tarefa);

                              index++;
                        }
                  }

                  Api.UpdateTarefaOrdem(tarefas).ContinueWith((task) =>
                  {
                        if (task.Status == TaskStatus.RanToCompletion)
                        {
                              // TODO tratar sucesso aqui

                        }
                        else if (task.IsFaulted)
                        {
                              // TODO tratar erro aqui

                        }
                  });
            }
      }
}