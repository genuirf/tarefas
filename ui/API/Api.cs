using Flurl.Http;
using shared.Model;
using shared.Model.Request;
using shared.Model.Response;

namespace tarefas.API
{
      public static class Api
      {

            public static async Task<GrupoSaveResponse>AddGrupo(Grupo grupo)
            {
                  var url = $"{App.api_url}/grupo/add";

                  var request = new GrupoSaveRequest();
                  request.grupo = grupo;

                 var result = await  url.PostJsonAsync(request);
                  var response = await result.GetJsonAsync<GrupoSaveResponse>();

                  return response;
            }

            public static async Task<GrupoSaveResponse> UpdateGrupo(Grupo grupo)
            {
                  var url = $"{App.api_url}/grupo/update";

                  var request = new GrupoSaveRequest();
                  request.grupo = grupo;

                 var result = await  url.PostJsonAsync(request);
                  var response = await result.GetJsonAsync<GrupoSaveResponse>();

                  return response;
            }
     
            public static async Task<GrupoListResponse> UpdateGrupoOrdem(List<Grupo> grupos)
            {
                  var url = $"{App.api_url}/grupo/update_ordem";

                  var request = new GrupoUpdateOrdemRequest();
                  request.grupos = grupos;

                  var result = await url.PostJsonAsync(request);
                  var response = await result.GetJsonAsync<GrupoListResponse>();

                  return response;
            }

            public static async Task<GrupoSaveResponse> DeleteGrupo(int Id)
            {
                  var url = $"{App.api_url}/grupo/delete/{Id}";

                 var result = await  url.DeleteAsync();
                  var response = await result.GetJsonAsync<GrupoSaveResponse>();

                  return response;
            }

            public static async Task<GrupoListResponse> ListGrupo()
            {
                  var url = $"{App.api_url}/grupo/list";

                 var result = await  url.GetAsync();
                  var response = await result.GetJsonAsync<GrupoListResponse>();

                  return response;
            }

            public static async Task<TarefaListResponse> ListTarefa(int grupo_Id)
            {
                  var url = $"{App.api_url}/tarefa/list_by_grupo/{grupo_Id}";

                 var result = await  url.GetAsync();
                  var response = await result.GetJsonAsync<TarefaListResponse>();

                  return response;
            }


            public static async Task<TarefaSaveResponse> AddTarefa(Tarefa tarefa)
            {
                  var url = $"{App.api_url}/tarefa/add";

                  var request = new TarefaSaveRequest();
                  request.tarefa = tarefa;

                  var result = await url.PostJsonAsync(request);
                  var response = await result.GetJsonAsync<TarefaSaveResponse>();

                  return response;
            }

            public static async Task<TarefaSaveResponse> UpdateTarefa(Tarefa tarefa)
            {
                  var url = $"{App.api_url}/tarefa/update";

                  var request = new TarefaSaveRequest();
                  request.tarefa = tarefa;

                  var result = await url.PostJsonAsync(request);
                  var response = await result.GetJsonAsync<TarefaSaveResponse>();

                  return response;
            }
     
            public static async Task<TarefaListResponse> UpdateTarefaOrdem(List<Tarefa> tarefas)
            {
                  var url = $"{App.api_url}/tarefa/update_ordem";

                  var request = new TarefaUpdateOrdemRequest();
                  request.tarefas = tarefas;

                  var result = await url.PostJsonAsync(request);
                  var response = await result.GetJsonAsync<TarefaListResponse>();

                  return response;
            }


      }
}
