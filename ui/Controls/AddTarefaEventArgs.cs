using shared.Model;
using tarefas.Model;

namespace tarefas.Controls
{
      public class AddTarefaEventArgs : EventArgs
      {
            public AddTarefaEventArgs(TarefaGrupo grupoParent, Tarefa? tarefa) 
            {
                  Grupo = grupoParent;
                  Tarefa = tarefa;
            }

            public TarefaGrupo Grupo { get; private set; }
            public Tarefa? Tarefa { get; private set; }
      }
}
