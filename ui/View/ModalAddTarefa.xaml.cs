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
                  var copia = new Tarefa();
                  copia.Id = tarefa.Id;
                  copia.ordem = tarefa.ordem;
                  copia.Titulo = tarefa.Titulo;
                  copia.Descricao = tarefa.Descricao;
                  copia.grupo_Id = tarefa.grupo_Id;
                  copia.DataCadastro = tarefa.DataCadastro;
                  copia.DataConclusao = tarefa.DataConclusao;
                  copia.concluido = tarefa.concluido;
                  copia.arquivado = tarefa.arquivado;

                  this.canvas_parent = canvas_parent;
                  this.model.original = tarefa;
                  this.model.tarefa = copia;
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
