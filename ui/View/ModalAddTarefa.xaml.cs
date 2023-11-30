using shared.Model;
using System.Windows;
using System.Windows.Controls;
using tarefas.API;
using tarefas.Controls;
using tarefas.Model;
using tarefas.ViewModel;

namespace tarefas.View
{
      /// <summary>
      /// Interação lógica para ModalAddTarefa.xam
      /// </summary>
      public partial class ModalAddTarefa : UserControl
      {
            private TarefaViewModel model;

            public ModalAddTarefa()
            {
                  InitializeComponent();

                  Loaded += ModalAddTarefa_Loaded;
                  Unloaded += ModalAddTarefa_Unloaded;

                  DataContext = model = new TarefaViewModel();

                  model.CloseEvent += Model_CloseEvent;
            }

            private void Model_CloseEvent(object? sender, EventArgs e)
            {
                  Close();
            }

            private void ModalAddTarefa_Unloaded(object sender, RoutedEventArgs e)
            {
                  Loaded -= ModalAddTarefa_Loaded;
                  Unloaded -= ModalAddTarefa_Unloaded;
            }

            private void ModalAddTarefa_Loaded(object sender, RoutedEventArgs e)
            {
                  TbTitulo.Focus();
            }

            public Panel canvas_parent { get; private set; }
            public void Show(Panel canvas_parent, string Title, Tarefa tarefa, TarefaGrupo grupoParent)
            {
                  this.canvas_parent = canvas_parent;
                  this.model.tarefa = tarefa;
                  this.model.GrupoParent = grupoParent;
                  this.TbTituloModal.Text = Title;

                  this.canvas_parent.Children.Add(this);
                
            }

            public void Close()
            {
                  this.canvas_parent.Children.Remove(this);
            }
      }
}
