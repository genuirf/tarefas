using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace shared.Model.Request
{
    public class GrupoSaveRequest : BaseRequest
    {

        [Required]
        public Grupo grupo { get; set; }
    }
}

