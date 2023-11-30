using shared.Model;
using tarefas.Model;

namespace tarefas.Controls
{
      public class EditarGrupoEventArgs : EventArgs
      {
            public EditarGrupoEventArgs(TarefaGrupo grupo) 
            {
                  Grupo = grupo;
            }

            public TarefaGrupo Grupo { get; private set; }
      }
}
