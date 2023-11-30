using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using Org.BouncyCastle.Ocsp;
using shared.Model;
using util;

namespace api.DAO
{
      public class GrupoDAO : MySqlDAO
      {
            public GrupoDAO(string schema, string strConn, MySqlTransaction? tr = null) : base(schema, "grupo", strConn, tr)
            {
            }

            public override bool Create(Model obj)
            {
                  Grupo reg = (Grupo)obj;

                  var cmd = MySQLx.NewCommand(this.schema, this.table);
                  cmd.SetConn(strConn, transaction);

                  cmd.Add("Descricao", reg.Descricao);
                  cmd.Add("ordem", reg.ordem);

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
                  Grupo reg = (Grupo)obj;

                  var cmd = MySQLx.NewSelect($"SELECT * FROM {this.table} WHERE Id = @Id");
                  cmd.SetConn(strConn, transaction);

                  cmd.Add("Id", reg.Id);

                  if (cmd.Primeiro())
                  {

                        reg.Descricao = cmd.String("Descricao", "");
                        reg.ordem = cmd.Int("ordem", 0);

                        return reg;
                  }

                  return null;
            }

            public override bool Update(Model obj)
            {
                  int result;
                  Grupo reg = (Grupo)obj;
                  var cmd = MySQLx.NewCommand(this.schema, this.table, "where Id = @Id");
                  cmd.SetConn(strConn, transaction);

                  cmd.Add("Id", reg.Id);
                  cmd.Add("Descricao", reg.Descricao);
                  cmd.Add("ordem", reg.ordem);

                  result = cmd.Update();

                  return result > 0;
            }

            public override bool Delete(Model obj)
            {
                  Grupo reg = (Grupo)obj;
                  var cmd = MySQLx.NewCommand(this.schema, this.table, "where Id = @Id");
                  cmd.SetConn(strConn, transaction);

                  cmd.Add("Id", reg.Id);

                  return cmd.Delete() > 0;
            }

            public override bool Salvar(Model obj)
            {
                  bool result;
                  Grupo reg = (Grupo)obj;

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

            public List<Grupo> GetAll()
            {
                  var list = new List<Grupo>();

                  var cmd = MySQLx.NewSelect($"SELECT * FROM {this.table} order by ordem");
                  cmd.SetConn(strConn, transaction);

                  while (cmd.Proximo())
                  {

                        Grupo reg = new();

                        reg.Id = cmd.Int("Id");
                        reg.Descricao = cmd.String("Descricao", "");
                        reg.ordem = cmd.Int("ordem", 0);

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

            public int UpdateOrdem(List<Grupo> grupos)
            {
                  int result = 0;

                  foreach (var reg in grupos)
                  {
                        var cmd = MySQLx.NewCommand(this.schema, this.table, "where Id = @Id");
                        cmd.SetConn(strConn, transaction);

                        cmd.Add("Id", reg.Id);
                        cmd.Add("ordem", reg.ordem);

                        result += cmd.Update();
                  }

                  return result;
            }
      }
}
