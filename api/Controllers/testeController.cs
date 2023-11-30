using shared.Model.Response;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
      [ApiController]
      [Route("[controller]")]
      public class testeController : ControllerBase
      {

            public AppConfig config { get; }


            public testeController(Microsoft.Extensions.Options.IOptions<AppConfig> config)
            {
                  this.config = config.Value;
            }

            [HttpGet("connection")]
            public BaseResponse Get()
            {
                  BaseResponse ret = new BaseResponse();

                  ret.msg = "<connected>";
                  ret.sucesso = true;
                  return ret;
            }
      }
}