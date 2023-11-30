using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace shared.Model
{
      public abstract class Model : INotifyPropertyChanged, INotifyPropertyChanging
      {
            public Model() : base()
            {
            }
            public void Clear()
            {
                  var k = valores.Keys.ToList();
                  this.valores.Clear();
                  k.ForEach(k1 => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(k1)));
            }
      
            [JsonIgnore]
            public List<string> todos_campos
            {
                  get
                  {
                        if (this.IsNew)
                        {
                              var l = new List<string>();

                              l = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance).Where(r => r.CanWrite).Select(r => r.Name).ToList();

                              return l;
                        }
                        else
                        {
                              return valores.Keys.ToList();
                        }
                  }
            }
            internal Dictionary<string, object?> valores = new Dictionary<string, object?>();
            public T? Get<T>([CallerMemberName] string campo = "")
            {
                  if (valores.ContainsKey(campo))
                        return (T?)valores[campo];
                  return default(T);
            }
            public void Set(object? valor, [CallerMemberName] string campo = "")
            {
                  if (campo == null || campo.Length < 1)
                        return;

                  PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(campo));
                  valores[campo] = valor;
                  PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(campo));
            }

            public int Id { get => Get<int>(); set => Set(value); }

            [JsonIgnore]
            public bool IsNew
            {
                  get
                  {
                        return Id <= 0;
                  }
            }

            public object? this[string campo]
            {
                  get
                  {
                        return Get<object?>(campo);
                  }
                  set
                  {
                        Set(value, campo);
                  }
            }
            public void OnPropertyChanging([CallerMemberName] string property = "")
            {
                  PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(property));
            }
            public void OnPropertyChanged([CallerMemberName] string property = "")
            {
                  PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
            }
            public event PropertyChangingEventHandler? PropertyChanging;
            public event PropertyChangedEventHandler? PropertyChanged;

            public void CopyFrom(Model row)
            {
                  var keys = valores.Keys.ToList();
                  valores.Clear();

                  foreach (string k in keys)
                  {
                        this[k] = row.valores[k];
                  }
            }
      }
}
