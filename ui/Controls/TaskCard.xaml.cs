using shared.Model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using tarefas.Model;
using tarefas.View;

namespace tarefas.Controls
{
      public partial class TaskCard : UserControl
      {
            private static TaskCard? taskCardMoving;
            public static TaskCard? TaskCardMoving { get => taskCardMoving; }

            public TaskColumn? TaskColumnParent
            {
                  get
                  {
                        DependencyObject? parent = VisualTreeHelper.GetParent(this);

                        while (parent != null && !(parent is TaskColumn))
                        {
                              parent = VisualTreeHelper.GetParent(parent);
                        }

                        return parent as TaskColumn;
                  }
            }


            private static AdornerLayer? adornerLayer;
            private static DragAdorner? _dragAdorner;
            private Point _initialMousePosition;
            private Point _initialAdornerPosition;


            public TaskCard()
            {
                  InitializeComponent();                  

                  this.Unloaded += TaskCard_Unloaded;
                  this.Loaded += TaskCard_Loaded;
            }

            private void TaskCard_Loaded(object sender, RoutedEventArgs e)
            {
                  this.PreviewMouseLeftButtonDown += TaskCard_MouseLeftButtonDown;
                  if (App.Current?.MainWindow != null) App.Current.MainWindow.PreviewMouseLeftButtonUp += TaskCard_MouseLeftButtonUp;
                  if (App.Current?.MainWindow != null) App.Current.MainWindow.PreviewMouseMove += TaskCard_PreviewMouseMove;
            }

            private void TaskCard_Unloaded(object sender, RoutedEventArgs e)
            {
                  this.Unloaded -= TaskCard_Unloaded;
                  this.Loaded -= TaskCard_Loaded;
                  this.PreviewMouseLeftButtonDown -= TaskCard_MouseLeftButtonDown;
                  if (App.Current?.MainWindow != null) App.Current.MainWindow.PreviewMouseLeftButtonUp -= TaskCard_MouseLeftButtonUp;
                  if (App.Current?.MainWindow != null) App.Current.MainWindow.PreviewMouseMove -= TaskCard_PreviewMouseMove;
            }

            private void TaskCard_PreviewMouseMove(object sender, MouseEventArgs e)
            {
                  if (taskCardMoving is null) return;

                  var currentPosition = e.GetPosition(((ListaTarefas)App.Current.MainWindow).scroll);

                  try
                  {
                        border.Margin = new(5);

                        var tarefa= (Tarefa)this.DataContext;
                        var index = TaskColumn.DestTarefaGrupo?.tarefas.IndexOf(tarefa) ?? -1;

                        var bounds = this.TransformToAncestor(((ListaTarefas)App.Current.MainWindow).scroll).TransformBounds(new Rect(0, 0, this.ActualWidth, this.ActualHeight));

                        if (bounds.Contains(currentPosition))
                        {
                              border.Margin = new(5, 20, 5, 5);
                              TaskColumn.SetDestTarefaIndex(index);
                        }
                        else if (index == TaskColumn.DestTarefaIndex) 
                        {
                              TaskColumn.SetDestTarefaIndex(-1);
                        }
                  }catch { }
            }

            private void TaskCard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
            {
                  App.Current.MainWindow.PreviewMouseMove += MainWindow_PreviewMouseMove;
                  App.Current.MainWindow.MouseLeave += MainWindow_MouseLeave;

                  taskCardMoving = this;

                  _initialMousePosition = e.GetPosition(this);
                  _initialAdornerPosition = e.GetPosition(border);

            }

            private void TaskCard_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
            {
                  App.Current.MainWindow.PreviewMouseMove -= MainWindow_PreviewMouseMove;

                  if (taskCardMoving != null)
                  {
                        TaskColumn.ComfirmMovTarefa();
                  }

                  RemoveAdorner();
            }

            private void MainWindow_MouseLeave(object sender, MouseEventArgs e)
            {
                  App.Current.MainWindow.PreviewMouseMove -= MainWindow_PreviewMouseMove;
                  App.Current.MainWindow.MouseLeave -= MainWindow_MouseLeave;
                  RemoveAdorner();
            }

            private void MainWindow_PreviewMouseMove(object sender, MouseEventArgs e)
            {
                  if (adornerLayer is null)
                  {
                        adornerLayer = AdornerLayer.GetAdornerLayer(((ListaTarefas)App.Current.MainWindow).scroll);
                  }
                  if (_dragAdorner is null && taskCardMoving != null)
                  {
                        var bitmap = new RenderTargetBitmap((int)taskCardMoving.border.ActualWidth, (int)taskCardMoving.border.ActualHeight, 96, 96, PixelFormats.Pbgra32);
                        bitmap.Render(taskCardMoving.border);

                        var image = new Image { Source = bitmap };
                        var dragRectangle = new Rectangle { Width = taskCardMoving.border.ActualWidth, Height = taskCardMoving.border.ActualHeight, Fill = new VisualBrush(image) };
                                                
                        _dragAdorner = new DragAdorner(((ListaTarefas)App.Current.MainWindow).scroll, dragRectangle);
                        adornerLayer.Add(_dragAdorner);

                        _dragAdorner.Offset = e.GetPosition(((ListaTarefas)App.Current.MainWindow).scroll);

                        if (TaskCardMoving is TaskCard taskCard && TaskCardMoving?.TaskColumnParent is TaskColumn taskColumn)
                        {
                              Tarefa tarefa = (Tarefa)taskCard.DataContext;
                              TarefaGrupo tarefaGrupo = (TarefaGrupo)taskColumn.DataContext;

                              TaskColumn.SetOrigTarefa(tarefa, tarefaGrupo);
                        }
                        else
                        {
                              TaskColumn.ClearMovTarefa();
                        }
                  }
                 
                  if (_dragAdorner != null)
                  {
                        var currentPosition = e.GetPosition(((ListaTarefas)App.Current.MainWindow).scroll);

                        // Calcula a diferença entre a posição inicial e a posição atual do cursor
                        var offsetX = currentPosition.X - _initialMousePosition.X;
                        var offsetY = currentPosition.Y - _initialMousePosition.Y;

                        // Calcula a nova posição do adorner considerando a diferença
                        var newPosition = new Point(_initialAdornerPosition.X + offsetX, _initialAdornerPosition.Y + offsetY);

                        _dragAdorner.UpdatePosition(newPosition);
                  }
            }

            private void RemoveAdorner()
            {
                  taskCardMoving = null;
                  border.Margin = new(5);

                  if (_dragAdorner != null && adornerLayer != null)
                  {
                        adornerLayer.Remove(_dragAdorner);
                        _dragAdorner = null;
                  }

                  TaskColumn.RevertMovTarefa();

            }
      }


}
