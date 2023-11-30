using System.Windows.Documents;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;

namespace tarefas.Controls
{
      public class DragAdorner : Adorner
      {
            private readonly Rectangle _dragRectangle;

            // Propriedade para armazenar o deslocamento
            public Point Offset { get; set; }

            public DragAdorner(UIElement adornedElement, Rectangle dragRectangle)
                : base(adornedElement)
            {
                  _dragRectangle = dragRectangle;
                  _dragRectangle.Fill.Opacity = 0.5;
                  Offset = new Point();

            }

            protected override void OnRender(DrawingContext drawingContext)
            {                  
                  var currentPosition = (Point)(Mouse.GetPosition(this) - new Point(_dragRectangle.Width / 2, _dragRectangle.Height / 2));

                  if (currentPosition.X < 0)
                  {
                        currentPosition.X = 0;
                  }
                  if (currentPosition.Y < 0)
                  {
                        currentPosition.Y = 0;
                  }

                  var rect = new Rect(currentPosition, new Size(_dragRectangle.Width, _dragRectangle.Height));
                  drawingContext.DrawRectangle(_dragRectangle.Fill, null, rect);
            }

            public void UpdatePosition(Point newPosition)
            {
                  Offset = newPosition;
                  InvalidateVisual();
            }
      }

}
