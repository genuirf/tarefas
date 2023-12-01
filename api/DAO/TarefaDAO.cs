using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using Org.BouncyCastle.Ocsp;
using shared.Model;
using util;

namespace api.DAO
{
      public class TarefaDAO : MySqlDAO
      {
            public TarefaDAO(string schema, string strConn, MySqlTransaction? tr = null) : base(schema, "tarefa", strConn, tr)
            {
            }

            public override bool Create(Model obj)
            {
                  Tarefa reg = (Tarefa)obj;

                  var cmd = MySQLx.NewCommand(this.schema, this.table);
                  cmd.SetConn(strConn, transaction);

                  cmd.Add("grupo_Id", reg.grupo_Id);
                  cmd.Add("Titulo", reg.Titulo);
                  cmd.Add("Descricao", reg.Descricao);
                  cmd.Add("ordem", reg.ordem);
                  cmd.Add("DataCadastro", reg.DataCadastro);
                  cmd.Add("DataConclusao", reg.DataConclusao);
                  cmd.Add("concluido", reg.concluido);
                  cmd.Add("arquivado", reg.arquivado);

                  var ret = cmd.InsertAndGetColumns(new[] { "Id" }, "Id");

                  if (ret != null)
                  {
                        reg.Id = (int)ret["Id"].value;

                        return true;
                  }

                  return false;
            }

            public override Model? Read(Model obj)
            {
                  Tarefa reg = (Tarefa)obj;

                  var cmd = MySQLx.NewSelect($"SELECT * FROM {this.table} WHERE Id = @Id");
                  cmd.SetConn(strConn, transaction);

                  cmd.Add("Id", reg.Id);

                  if (cmd.Primeiro())
                  {

                        reg.grupo_Id = cmd.Int("grupo_Id");
                        reg.Titulo = cmd.String("Titulo", "");
                        reg.Descricao = cmd.String("Descricao", "");
                        reg.ordem = cmd.Int("ordem", 0);
                        reg.DataCadastro = cmd.Date("DataCadastro");
                        reg.DataConclusao = cmd.Date("DataConclusao");
                        reg.concluido = cmd.Boolean("concluido");
                        reg.arquivado = cmd.Boolean("arquivado");

                        return reg;
                  }

                  return null;
            }

            public override bool Update(Model obj)
            {
                  int result;
                  Tarefa reg = (Tarefa)obj;
                  var cmd = MySQLx.NewCommand(this.schema, this.table, "where Id = @Id");
                  cmd.SetConn(strConn, transaction);

                  cmd.Add("Id", reg.Id);
                  cmd.Add("grupo_Id", reg.grupo_Id);
                  cmd.Add("Titulo", reg.Titulo);
                  cmd.Add("Descricao", reg.Descricao);
                  cmd.Add("ordem", reg.ordem);
                  cmd.Add("DataCadastro", reg.DataCadastro);
                  cmd.Add("DataConclusao", reg.DataConclusao);
                  cmd.Add("concluido", reg.concluido);
                  cmd.Add("arquivado", reg.arquivado);

                  result = cmd.Update();

                  return result > 0;
            }

            public override bool Delete(Model obj)
            {
                  Tarefa reg = (Tarefa)obj;
                  var cmd = MySQLx.NewCommand(this.schema, this.table, "where Id = @Id");
                  cmd.SetConn(strConn, transaction);

                  cmd.Add("Id", reg.Id);

                  return cmd.Delete() > 0;
            }

            public override bool Salvar(Model obj)
            {
                  bool result;
                  Tarefa reg = (Tarefa)obj;

                  if (reg.Id > 0)
                  {
                        result = Update(obj);
                  }
                  else
                  {
                        result = Create(obj);
                  }

                  return result;
            }

            // outras funções aqui

            public List<Tarefa> GetAll()
            {
                  var list = new List<Tarefa>();

                  var cmd = MySQLx.NewSelect($"SELECT * FROM {this.table} order by ordem");
                  cmd.SetConn(strConn, transaction);

                  while (cmd.Proximo())
                  {

                        Tarefa reg = new();

                        reg.Id = cmd.Int("Id");
                        reg.grupo_Id = cmd.Int("grupo_Id");
                        reg.Titulo = cmd.String("Titulo", "");
                        reg.Descricao = cmd.String("Descricao", "");
                        reg.ordem = cmd.Int("ordem", 0);
                        reg.DataCadastro = cmd.Date("DataCadastro");
                        reg.DataConclusao = cmd.Date("DataConclusao");
                        reg.concluido = cmd.Boolean("concluido");
                        reg.arquivado = cmd.Boolean("arquivado");

                        list.Add(reg);
                  }

                  return list;
            }
            public List<Tarefa> GetAllByGrupo(int grupo_Id)
            {
                  var list = new List<Tarefa>();

                  var cmd = MySQLx.NewSelect($"SELECT * FROM {this.table} where grupo_Id = @grupo_Id order by ordem");
                  cmd.SetConn(strConn, transaction);
                  cmd.Add("grupo_Id", grupo_Id);

                  while (cmd.Proximo())
                  {

                        Tarefa reg = new();

                        reg.Id = cmd.Int("Id");
                        reg.grupo_Id = cmd.Int("grupo_Id");
                        reg.Titulo = cmd.String("Titulo", "");
                        reg.Descricao = cmd.String("Descricao", "");
                        reg.ordem = cmd.Int("ordem", 0);
                        reg.DataCadastro = cmd.Date("DataCadastro");
                        reg.DataConclusao = cmd.Date("DataConclusao");
                        reg.concluido = cmd.Boolean("concluido");
                        reg.arquivado = cmd.Boolean("arquivado");

                        list.Add(reg);
                  }

                  return list;
            }
            public int GetCount()
            {

                  var cmd = MySQLx.NewSelect($"SELECT count(*) n FROM {this.table}");
                  cmd.SetConn(strConn, transaction);
                  cmd.Primeiro();

                  return cmd.Int("n");
            }
            public int GetCountByGrupo(int grupo_Id)
            {

                  var cmd = MySQLx.NewSelect($"SELECT count(*) n FROM {this.table} where grupo_Id = @grupo_Id");
                  cmd.SetConn(strConn, transaction);
                  cmd.Add("grupo_Id", grupo_Id);
                  cmd.Primeiro();

                  return cmd.Int("n");
            }

            public int UpdateOrdem(List<Tarefa> tarefas)
            {
                  int result = 0;

                  foreach (var reg in tarefas)
                  {
                        var cmd = MySQLx.NewCommand(this.schema, this.table, "where Id = @Id");
                        cmd.SetConn(strConn, transaction);

                        cmd.Add("Id", reg.Id);
                        cmd.Add("grupo_Id", reg.grupo_Id);
                        cmd.Add("ordem", reg.ordem);

                        result += cmd.Update();
                  }

                  return result;
            }
      }
}
