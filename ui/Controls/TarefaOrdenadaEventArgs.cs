using shared.Model;
using tarefas.Model;

namespace tarefas.Controls
{
      public class TarefaOrdenadaEventArgs : EventArgs
      {
            public TarefaOrdenadaEventArgs(List<TarefaGrupo> grupos) 
            {
                  Grupos = grupos;
            }

            /// <summary>
            /// Grupos envolvidos na ordenação/movimentação da tarefa
            /// </summary>
            public List<TarefaGrupo> Grupos { get; private set; }
      }
}
