using shared.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Shapes;
using tarefas.View;
using tarefas.Model;

namespace tarefas.Controls
{
      /// <summary>
      /// Interação lógica para TaskColumn.xam
      /// </summary>
      public partial class TaskColumn : UserControl
      {

            private static int _origTarefaIndex = -1;
            public static int OrigTarefaIndex { get => _origTarefaIndex; }
            public static void SetOrigTarefaIndex(int index)
            {
                  _origTarefaIndex = index;
            }

            private static int _destTarefaIndex = -1;
            public static int DestTarefaIndex { get => _destTarefaIndex; }
            public static void SetDestTarefaIndex(int index)
            {
                  _destTarefaIndex = index;
            }

            private static Tarefa? _origTarefa;
            public static Tarefa? OrigTarefa { get => _origTarefa; }
            public static void SetOrigTarefa(Tarefa tarefa, TarefaGrupo tarefaGrupo)
            {
                  _origTarefa = tarefa;
                  _origTarefaGrupo = tarefaGrupo;

                  _origTarefaIndex = tarefaGrupo.tarefas.IndexOf(tarefa);

                  tarefaGrupo.tarefas.RemoveAt(OrigTarefaIndex);
            }

            public static void ClearMovTarefa()
            {
                  _origTarefa = null;
                  _origTarefaGrupo = null;
                  _destTarefaGrupo = null;
                  _origTarefaIndex = -1;
                  _destTarefaIndex = -1;
            }
            public static List<TarefaGrupo> ComfirmMovTarefa()
            {
                  List<TarefaGrupo> list = new();
                  if (OrigTarefaGrupo != null) list.Add(OrigTarefaGrupo);

                  var index = DestTarefaIndex;
                  if (index < 0 && DestTarefaGrupo is TarefaGrupo tarefaGrupo1 && OrigTarefa is Tarefa tarefa1)
                  {
                        if(!list.Contains(tarefaGrupo1)) list.Add(tarefaGrupo1);

                        tarefa1.grupo_Id = tarefaGrupo1.Id;

                        tarefaGrupo1.tarefas.Add(tarefa1);
                        ClearMovTarefa();
                  }
                  else if (DestTarefaGrupo is TarefaGrupo tarefaGrupo  && OrigTarefa is Tarefa tarefa)
                  {
                        if (!list.Contains(tarefaGrupo)) list.Add(tarefaGrupo);

                        tarefa.grupo_Id = tarefaGrupo.Id;

                        tarefaGrupo.tarefas.Insert(Math.Min(index, tarefaGrupo.tarefas.Count), tarefa);
                        ClearMovTarefa();
                  }
                  
                  return list;
            }
            public static void RevertMovTarefa()
            {
                  if (OrigTarefaIndex > -1 && OrigTarefa != null && OrigTarefaGrupo != null)
                  {
                        OrigTarefaGrupo.tarefas.Insert(OrigTarefaIndex, OrigTarefa);
                        ClearMovTarefa();
                  }
            }

            #region Movimentação de Colunas


            private static TarefaGrupo? _destTarefaGrupo;
            public static TarefaGrupo? DestTarefaGrupo { get => _destTarefaGrupo; }
            public static void SetDestTarefaGrupo(TarefaGrupo? tarefaGrupo)
            {
                  _destTarefaGrupo = tarefaGrupo;
            }

            private static TarefaGrupo? _origTarefaGrupo;
            public static TarefaGrupo? OrigTarefaGrupo { get => _origTarefaGrupo; }
            public static void SetOrigTarefaGrupo(TarefaGrupo? tarefaGrupo)
            {
                  _origTarefaGrupo = tarefaGrupo;

                  if (tarefaGrupo != null)
                  {
                        _origTarefaGrupoIndex = tarefaGrupo.parent.IndexOf(tarefaGrupo);
                        tarefaGrupo.parent.RemoveAt(OrigTarefaGrupoIndex);
                  }
            }

            private static int _origTarefaGrupoIndex = -1;
            public static int OrigTarefaGrupoIndex { get => _origTarefaGrupoIndex; }
            public static void SetOrigTarefaGrupoIndex(int index)
            {
                  _origTarefaGrupoIndex = index;
            }


            private static int _destTarefaGrupoIndex = -1;
            public static int DestTarefaGrupoIndex { get => _destTarefaGrupoIndex; }
            public static void SetDestTarefaGrupoIndex(int index)
            {
                  _destTarefaGrupoIndex = index;
            }



            public static void ClearMovTarefaGrupo()
            {
                  _origTarefaGrupo = null;
                  _origTarefaGrupoIndex = -1;
                  _destTarefaGrupoIndex = -1;
            }
            public static void ComfirmMovTarefaGrupo()
            {
                  var index = DestTarefaGrupoIndex;
                  if (index < 0 && OrigTarefaGrupo is TarefaGrupo tarefaGrupo)
                  {
                        tarefaGrupo.parent.Add(tarefaGrupo);
                        ClearMovTarefaGrupo();
                  }
                  else if (OrigTarefaGrupo is TarefaGrupo tarefaGrupo1)
                  {
                        tarefaGrupo1.parent.Insert(Math.Min(index, tarefaGrupo1.parent.Count), tarefaGrupo1);
                        ClearMovTarefaGrupo();
                  }

            }
            public static void RevertMovTarefaGrupo()
            {
                  if (OrigTarefaGrupoIndex > -1 && OrigTarefaGrupo != null )
                  {
                        OrigTarefaGrupo.parent.Insert(OrigTarefaGrupoIndex, OrigTarefaGrupo);
                        ClearMovTarefaGrupo();
                  }
            }

            #endregion

            private static AdornerLayer? adornerLayer;
            private static DragAdorner? _dragAdorner;
            private Point _initialMousePosition;
            private Point _initialAdornerPosition;

            private static TaskColumn? taskColumnMoving;
            public static TaskColumn? TaskColumnMoving { get => taskColumnMoving; }

            public TaskColumn()
            {
                  InitializeComponent();

                  this.Unloaded += TaskCard_Unloaded;
                  this.Loaded += TaskCard_Loaded;

                  
            }

            private void TaskCard_Loaded(object sender, RoutedEventArgs e)
            {
                  this.title.PreviewMouseLeftButtonDown += TaskColumn_MouseLeftButtonDown;
                  if (App.Current?.MainWindow != null) App.Current.MainWindow.PreviewMouseLeftButtonUp += TaskColumn_MouseLeftButtonUp;
                  if (App.Current?.MainWindow != null) App.Current.MainWindow.PreviewMouseMove += TaskColumn_PreviewMouseMove;
            }

            private void TaskCard_Unloaded(object sender, RoutedEventArgs e)
            {
                  this.Unloaded -= TaskCard_Unloaded;
                  this.Loaded -= TaskCard_Loaded;
                  if (this.title != null) this.title.PreviewMouseLeftButtonDown -= TaskColumn_MouseLeftButtonDown;
                  if (App.Current?.MainWindow != null) App.Current.MainWindow.PreviewMouseLeftButtonUp -= TaskColumn_MouseLeftButtonUp;
                  if (App.Current?.MainWindow != null) App.Current.MainWindow.PreviewMouseMove -= TaskColumn_PreviewMouseMove;
            }

            private void TaskColumn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
            {
                  App.Current.MainWindow.PreviewMouseMove += MainWindow_PreviewMouseMove;
                  App.Current.MainWindow.MouseLeave += MainWindow_MouseLeave;

                  taskColumnMoving = this;

                  _initialMousePosition = e.GetPosition(this);
                  _initialAdornerPosition = e.GetPosition(border);

            }

            private void TaskColumn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
            {
                  App.Current.MainWindow.PreviewMouseMove -= MainWindow_PreviewMouseMove;

                  if (taskColumnMoving != null)
                  {
                        ComfirmMovTarefaGrupo();
                        GrupoOrdenadoEvent?.Invoke(this, EventArgs.Empty);
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
                  if (_dragAdorner is null && taskColumnMoving != null)
                  {
                        var bitmap = new RenderTargetBitmap((int)taskColumnMoving.border.ActualWidth, (int)taskColumnMoving.border.ActualHeight, 96, 96, PixelFormats.Pbgra32);
                        bitmap.Render(taskColumnMoving.border);

                        var image = new Image { Source = bitmap };
                        var dragRectangle = new Rectangle { Width = taskColumnMoving.border.ActualWidth, Height = taskColumnMoving.border.ActualHeight, Fill = new VisualBrush(image) };

                        _dragAdorner = new DragAdorner(((ListaTarefas)App.Current.MainWindow).scroll, dragRectangle);
                        adornerLayer.Add(_dragAdorner);

                        _dragAdorner.Offset = e.GetPosition(((ListaTarefas)App.Current.MainWindow).scroll);

                        if (TaskColumnMoving is TaskColumn taskColumn)
                        {
                              TarefaGrupo tarefaGrupo = (TarefaGrupo)taskColumn.DataContext;

                              SetOrigTarefaGrupo(tarefaGrupo);
                        }
                        else
                        {
                              ClearMovTarefaGrupo();
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

            private void TaskColumn_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
            {
                  var currentPosition = e.GetPosition(((ListaTarefas)App.Current.MainWindow).scroll);

                  try
                  {
                        border.Margin = new(2);
                        var tarefaGrupo = (TarefaGrupo)this.DataContext;
                        var index = tarefaGrupo?.parent.IndexOf(tarefaGrupo) ?? -1;

                        var bounds = this.TransformToAncestor(((ListaTarefas)App.Current.MainWindow).scroll).TransformBounds(new Rect(0, 0, this.ActualWidth, this.ActualHeight));

                        if (bounds.Contains(currentPosition))
                        {
                              if (taskColumnMoving != null)
                              {
                                    border.Margin = new(40, 2, 2, 2);
                                    SetDestTarefaGrupoIndex(index);
                              }

                              SetDestTarefaGrupo(tarefaGrupo);
                        }
                        else
                        {
                              if (index == DestTarefaGrupoIndex)
                              {
                                    SetDestTarefaGrupoIndex(-1);
                              }

                              if (tarefaGrupo == DestTarefaGrupo)
                              {
                                    SetDestTarefaGrupo(null);
                              }
                        }

                  }
                  catch { }
                  
            }

            private void RemoveAdorner()
            {
                  taskColumnMoving = null;
                  border.Margin = new(2);

                  if (_dragAdorner != null && adornerLayer != null)
                  {
                        adornerLayer.Remove(_dragAdorner);
                        _dragAdorner = null;
                  }

                  TaskColumn.RevertMovTarefaGrupo();

            }



            private void BtMenuGrupo_Click(object sender, RoutedEventArgs e)
            {
                  BtMenuGrupo.ContextMenu.IsOpen = true;
            }

            private void BtEditarGrupo_Click(object sender, RoutedEventArgs e)
            {
                  TarefaGrupo g = (TarefaGrupo)this.DataContext;
                  EditarGrupoEvent?.Invoke(this, new EditarGrupoEventArgs(g));
            }

            private void BtExcluirGrupo_Click(object sender, RoutedEventArgs e)
            {
                  TarefaGrupo g = (TarefaGrupo)this.DataContext;
                  ExcluirGrupoEvent?.Invoke(this, new EditarGrupoEventArgs(g));
            }

            private void BtAddTarefa_Click(object sender, RoutedEventArgs e)
            {
                  TarefaGrupo g = (TarefaGrupo)this.DataContext;
                  AddTarefaEvent?.Invoke(this, new AddEditTarefaEventArgs(g, null));
            }

            private void TaskCard_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
            {
                  TarefaGrupo g = (TarefaGrupo)this.DataContext;
                  Tarefa tarefa = (Tarefa)((FrameworkElement)sender).DataContext;
                  EditTarefaEvent?.Invoke(this, new AddEditTarefaEventArgs(g, tarefa));
            }

            public void OnTarefaOrdenadaEvent(List<TarefaGrupo> grupos)
            {
                  TarefaOrdenadaEvent?.Invoke(this, new TarefaOrdenadaEventArgs(grupos));
            }

            public event EventHandler<EditarGrupoEventArgs> EditarGrupoEvent;
            public event EventHandler<EditarGrupoEventArgs> ExcluirGrupoEvent;
            public event EventHandler<AddEditTarefaEventArgs> AddTarefaEvent;
            public event EventHandler<AddEditTarefaEventArgs> EditTarefaEvent;
            public event EventHandler GrupoOrdenadoEvent;
            public event EventHandler<TarefaOrdenadaEventArgs> TarefaOrdenadaEvent;

    }
}
