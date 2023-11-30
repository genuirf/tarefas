using shared.Model;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace tarefas.Model
{
      public class TarefaGrupo : Grupo
      {
            public TarefaGrupo(ObservableCollection<TarefaGrupo> parent) {
                  _tarefas = new();
                  _parent = parent;
            }


            private ObservableCollection<TarefaGrupo> _parent;
            [JsonIgnore]
            public ObservableCollection<TarefaGrupo> parent { get { return _parent; } }

            private ObservableCollection<Tarefa> _tarefas;
            [JsonIgnore]
            public ObservableCollection<Tarefa> tarefas { get { return _tarefas; } set { _tarefas = value; } }


            public override string ToString()
            {
                  return $"{Descricao} -> {tarefas.Count}";
            }
      }
}
