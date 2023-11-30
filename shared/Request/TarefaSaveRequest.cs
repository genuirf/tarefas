using System.ComponentModel.DataAnnotations;

namespace shared.Model.Request
{
    public class TarefaSaveRequest : BaseRequest
    {

        [Required]
        public Tarefa tarefa { get; set; }
    }
}

