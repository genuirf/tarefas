namespace shared.Model.Response
{
      public class TarefaListResponse : BaseResponse
      {
            public TarefaListResponse()
            {
                  tarefas = new();
            }
            public List<Tarefa> tarefas { get; set; }
      }
}

