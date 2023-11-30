
using MySql.Data.MySqlClient;
using shared.Model;

namespace api.DAO
{
    public abstract class MySqlDAO
      {

            private string schema_;
            public string schema
            {
                  get
                  {
                        return schema_;
                  }
            }

            private string table_;
            public string table
            {
                  get
                  {
                        return table_;
                  }
            }

            public MySqlTransaction? transaction { get; private set; }
            protected string strConn;

            private bool fecharConn { get; set; } = true;

            public MySqlTransaction BeguinTransaction()
            {
                  var conn = new MySqlConnection(strConn);
                  conn.Open();
                  transaction = conn.BeginTransaction();
                  fecharConn = false;
                  return transaction;
            }

            public void Commit()
            {
                  if (transaction != null)
                  {
                        transaction.Commit();
                  }

            }

            public void Rollback()
            {
                  try
                  {
                        if (transaction != null)
                        {
                              transaction?.Rollback();
                        }
                  }
                  catch { }
            }

            public MySqlDAO(string schema, string table, string strConn, MySqlTransaction? tr = null)
            {
                  this.schema_ = schema;
                  this.table_ = table;

                  this.strConn = strConn;
                  this.transaction = tr;
                  this.fecharConn = this.transaction == null;
            }

            public abstract bool Create(Model obj);
            public abstract Model? Read(Model obj);
            public abstract bool Update(Model obj);
            public abstract bool Delete(Model obj);

            public abstract bool Salvar(Model obj);

      }

}
