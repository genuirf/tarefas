using shared.Model.Request;
using shared.Model.Response;
using Microsoft.AspNetCore.Mvc;
using api.DAO;

namespace api.Controllers
{
      [ApiController]
      [Route("[controller]")]
      public class grupoController : ControllerBase
      {

            public AppConfig config { get; }


            public grupoController(Microsoft.Extensions.Options.IOptions<AppConfig> config)
            {
                  this.config = config.Value;
            }

            [HttpPost("add")]
            public GrupoSaveResponse Add([FromBody] GrupoSaveRequest info)
            {
                  GrupoSaveResponse ret = new GrupoSaveResponse();
                  ret.grupo = info.grupo;
                  try
                  {
                        var dao = new GrupoDAO(config.schema, config.connString);

                        info.grupo.ordem = dao.GetCount();

                        if (dao.Create(info.grupo))
                        {
                              ret.msg = "Registro salvo.";
                              ret.sucesso = true;
                        }
                        else
                        {
                              ret.msg = "Não foi possível salvar.";
                        }
                  }
                  catch (Exception ex)
                  {
                        ret.msg = ex.Message;
                        ret.sucesso = false;
                  }

                  return ret;
            }

            [HttpPost("update")]
            public GrupoSaveResponse Update([FromBody] GrupoSaveRequest info)
            {
                  GrupoSaveResponse ret = new GrupoSaveResponse();
                  ret.grupo = info.grupo;
                  try
                  {
                        var dao = new GrupoDAO(config.schema, config.connString);

                        if (dao.Update(info.grupo))
                        {
                              ret.msg = "Registro salvo.";
                              ret.sucesso = true;
                        }
                        else
                        {
                              ret.msg = "Não foi possível salvar.";
                        }
                  }
                  catch (Exception ex)
                  {
                        ret.msg = ex.Message;
                        ret.sucesso = false;
                  }

                  return ret;
            }

            [HttpPost("update_ordem")]
            public GrupoListResponse UpdateOrdem([FromBody] GrupoUpdateOrdemRequest info)
            {
                  GrupoListResponse ret = new GrupoListResponse();
                  ret.grupos = info.grupos;

                  var dao = new GrupoDAO(config.schema, config.connString);
                  try
                  {
                        dao.BeguinTransaction();

                        dao.UpdateOrdem(info.grupos);

                        dao.Commit();

                  }
                  catch (Exception ex)
                  {
                        dao.Rollback();

                        ret.msg = ex.Message;
                        ret.sucesso = false;
                  }

                  return ret;
            }

            [HttpDelete("delete/{Id}")]
            public GrupoSaveResponse Delete(int Id)
            {
                  GrupoSaveResponse ret = new GrupoSaveResponse();
                  try
                  {
                        var dao = new GrupoDAO(config.schema, config.connString);

                        ret.grupo = new();
                        ret.grupo.Id = Id;

                        dao.Read(ret.grupo);

                        if (dao.Delete(ret.grupo))
                        {
                              ret.msg = $"Registro excluído.";
                              ret.sucesso = true;
                        }
                        else
                        {
                              ret.msg = "Não foi possível excluir registro.";
                        }

                  }
                  catch (Exception ex)
                  {
                        ret.msg = ex.Message;
                        ret.sucesso = false;
                  }

                  return ret;
            }

            [HttpGet("list")]
            public GrupoListResponse List()
            {
                  GrupoListResponse ret = new GrupoListResponse();
                  try
                  {
                        var dao = new GrupoDAO(config.schema, config.connString);

                        ret.grupos = dao.GetAll();

                        ret.msg = $"Encontrado {ret.grupos.Count} registro(s).";
                        ret.sucesso = true;

                  }
                  catch (Exception ex)
                  {
                        ret.msg = ex.Message;
                        ret.sucesso = false;
                  }

                  return ret;
            }
      }
}