using System.ComponentModel.DataAnnotations;

namespace shared.Model.Request
{
      public class GrupoUpdateOrdemRequest : BaseRequest
      {
            [Required]
            public List<Grupo> grupos { get; set; }
      }
}
