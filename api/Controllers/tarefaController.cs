using shared.Model.Request;
using shared.Model.Response;
using Microsoft.AspNetCore.Mvc;
using api.DAO;

namespace api.Controllers
{
      [ApiController]
      [Route("[controller]")]
      public class tarefaController : ControllerBase
      {

            public AppConfig config { get; }


            public tarefaController(Microsoft.Extensions.Options.IOptions<AppConfig> config)
            {
                  this.config = config.Value;
            }

            [HttpPost("add")]
            public TarefaSaveResponse Add([FromBody] TarefaSaveRequest info)
            {
                  TarefaSaveResponse ret = new TarefaSaveResponse();
                  ret.tarefa = info.tarefa;
                  try
                  {
                        var dao = new TarefaDAO(config.schema, config.connString);

                        info.tarefa.ordem = dao.GetCountByGrupo(info.tarefa.grupo_Id ?? 0);

                        if (dao.Create(info.tarefa))
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
            public TarefaSaveResponse Update([FromBody] TarefaSaveRequest info)
            {
                  TarefaSaveResponse ret = new TarefaSaveResponse();
                  ret.tarefa = info.tarefa;
                  try
                  {
                        var dao = new TarefaDAO(config.schema, config.connString);

                        if (dao.Update(info.tarefa))
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
            public TarefaListResponse UpdateOrdem([FromBody] TarefaUpdateOrdemRequest info)
            {
                  TarefaListResponse ret = new TarefaListResponse();
                  ret.tarefas = info.tarefas;

                  var dao = new TarefaDAO(config.schema, config.connString);
                  try
                  {
                        dao.BeguinTransaction();

                        dao.UpdateOrdem(info.tarefas);

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
            public TarefaSaveResponse Delete(int Id)
            {
                  TarefaSaveResponse ret = new TarefaSaveResponse();
                  try
                  {
                        var dao = new TarefaDAO(config.schema, config.connString);

                        ret.tarefa = new();
                        ret.tarefa.Id = Id;

                        dao.Read(ret.tarefa);

                        if (dao.Delete(ret.tarefa))
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
            public TarefaListResponse List()
            {
                  TarefaListResponse ret = new TarefaListResponse();
                  try
                  {
                        var dao = new TarefaDAO(config.schema, config.connString);

                        ret.tarefas = dao.GetAll();

                        ret.msg = $"Encontrado {ret.tarefas.Count} registro(s).";
                        ret.sucesso = true;

                  }
                  catch (Exception ex)
                  {
                        ret.msg = ex.Message;
                        ret.sucesso = false;
                  }

                  return ret;
            }

            [HttpGet("list_by_grupo/{grupo_Id}")]
            public TarefaListResponse List(int grupo_Id)
            {
                  TarefaListResponse ret = new TarefaListResponse();
                  try
                  {
                        var dao = new TarefaDAO(config.schema, config.connString);

                        ret.tarefas = dao.GetAllByGrupo(grupo_Id);

                        ret.msg = $"Encontrado {ret.tarefas.Count} registro(s).";
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