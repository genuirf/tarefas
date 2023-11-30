namespace shared.Model.Response
{
      public class GrupoListResponse : BaseResponse
      {
            public GrupoListResponse()
            {
                  grupos = new();
            }
            public List<Grupo> grupos { get; set; }
      }
}

