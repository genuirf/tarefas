using shared.Model;
using System.Windows;
using System.Windows.Controls;
using tarefas.API;
using tarefas.Model;

namespace tarefas.View
{
      /// <summary>
      /// Interação lógica para ModalAddGrupo.xam
      /// </summary>
      public partial class ModalAddGrupo : UserControl
      {
            public ModalAddGrupo()
            {
                  InitializeComponent();

                  Loaded += ModalAddGrupo_Loaded;
                  Unloaded += ModalAddGrupo_Unloaded;
            }

            private void ModalAddGrupo_Unloaded(object sender, RoutedEventArgs e)
            {
                  Loaded -= ModalAddGrupo_Loaded;
                  Unloaded -= ModalAddGrupo_Unloaded;
            }

            private void ModalAddGrupo_Loaded(object sender, RoutedEventArgs e)
            {
                  TbGrupo.Focus();
            }

            private Action<TarefaGrupo>? onSave;
            private Action? onCancel;
            private TarefaGrupo grupo;
            public Panel canvas_parent { get; private set; }
            public void Show(Panel canvas_parent, string Title, TarefaGrupo grupo, Action<TarefaGrupo>? onSave = null, Action? onCancel = null)
            {
                  this.canvas_parent = canvas_parent;
                  this.onSave = onSave;
                  this.onCancel = onCancel;
                  this.grupo = grupo;
                  this.TbTitulo.Text = Title;

                  this.TbGrupo.Text = grupo.Descricao;

                  this.canvas_parent.Children.Add(this);

                
            }

            public void Close()
            {
                  this.canvas_parent.Children.Remove(this);
            }

            private void BtSalvar_Click(object sender, RoutedEventArgs e)
            {           
                  var obj = new Grupo()
                  {
                        Id = grupo.Id,
                        Descricao = TbGrupo.Text,
                        ordem = grupo.ordem ?? 0,
                  };
                  if (grupo.IsNew)
                  {
                        Api.AddGrupo(obj)
                            .ContinueWith((task) =>
                            {
                                  if (task.Status == TaskStatus.RanToCompletion)
                                  {
                                        grupo.Id = task.Result.grupo.Id; // resgatar Id inserido no registro
                                        grupo.Descricao = task.Result.grupo.Descricao;
                                        grupo.ordem = task.Result.grupo.ordem;

                                        Dispatcher.Invoke(() =>
                                        {
                                              onSave?.Invoke(grupo);
                                              Close();
                                        });
                                  }
                                  else if (task.IsFaulted)
                                  {
                                        // TODO tratar erro aqui

                                  }
                            });
                  }
                  else
                  {
                        Api.UpdateGrupo(obj)
                            .ContinueWith((task) =>
                            {
                                  if (task.Status == TaskStatus.RanToCompletion)
                                  {
                                        grupo.Descricao = task.Result.grupo.Descricao;
                                        grupo.ordem = task.Result.grupo.ordem;

                                        Dispatcher.Invoke(() =>
                                        {
                                              onSave?.Invoke(grupo);
                                              Close();
                                        });
                                  }
                                  else if (task.IsFaulted)
                                  {
                                        // TODO tratar erro aqui

                                  }
                            });
                  }
            }

            private void BtCancelar_Click(object sender, RoutedEventArgs e)
            {
                  onCancel?.Invoke();
                  Close();
            }
      }
}
