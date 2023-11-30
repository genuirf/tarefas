using shared.Model;
using System.Windows;
using System.Windows.Controls;
using tarefas.API;
using tarefas.Model;

namespace tarefas.View
{
      /// <summary>
      /// Interação lógica para ModalConfirma.xam
      /// </summary>
      public partial class ModalConfirma : UserControl
      {
            public ModalConfirma()
            {
                  InitializeComponent();
            }

            private Action onConfirm;
            private Action? onCancel;
            public Panel canvas_parent { get; private set; }
            public void Show(Panel canvas_parent, string Message, Action onConfirm, Action? onCancel = null)
            {
                  this.canvas_parent = canvas_parent;
                  this.onConfirm = onConfirm;
                  this.onCancel = onCancel;
                  this.TbMessage.Text = Message;

                  this.canvas_parent.Children.Add(this);
                
            }

            public void Close()
            {
                  this.canvas_parent.Children.Remove(this);
            }

            private void BtConfirm_Click(object sender, RoutedEventArgs e)
            {
                  onConfirm?.Invoke();
                  Close();                 
            }

            private void BtCancel_Click(object sender, RoutedEventArgs e)
            {
                  onCancel?.Invoke();
                  Close();
            }
      }
}
