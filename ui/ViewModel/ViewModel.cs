using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace tarefas.ViewModel
{
      public class ViewModel : INotifyPropertyChanged
      {
            public void OnPropertyChanged([CallerMemberName] string property = "")
            {
                  PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
            }
            public event PropertyChangedEventHandler? PropertyChanged;
      }
}
