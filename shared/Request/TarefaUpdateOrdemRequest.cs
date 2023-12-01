using System.ComponentModel.DataAnnotations;

namespace shared.Model.Request
{
      public class TarefaUpdateOrdemRequest : BaseRequest
      {
            [Required]
            public List<Tarefa> tarefas { get; set; }
      }
}
