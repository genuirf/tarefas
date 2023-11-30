using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace util
{
      public class AsyncCommand<T> : ICommand
      {
            private readonly Func<T?, Task> _execute;
            private readonly Func<T?, bool>? _canExecute;
            private bool _isExecuting;

            public event EventHandler? CanExecuteChanged;

            public AsyncCommand(Func<T?, Task> execute, Func<T?, bool>? canExecute = null)
            {
                  _execute = execute ?? throw new ArgumentNullException(nameof(execute));
                  _canExecute = canExecute;
            }

            public bool CanExecute(object? parameter)
            {
                  return !_isExecuting && (_canExecute == null || _canExecute((T?)parameter));
            }

            public async void Execute(object? parameter)
            {
                  if (!_isExecuting)
                  {
                        _isExecuting = true;
                        CanExecuteChanged?.Invoke(this, EventArgs.Empty);

                        try
                        {
                              await _execute((T?)parameter);
                        }
                        finally
                        {
                              _isExecuting = false;
                              CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                        }
                  }
            }
      }
}
