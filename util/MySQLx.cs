using System.Data;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using static util.funcoes;

namespace util
{
      public class MySQLxException : Exception
      {
            public MySQLxException(Exception? inner) : base(null, inner)
            {
            }
      }
      public class MySQLx
      {
            public static string? strConn { get;set; }
            public static Command NewCommand(string schema, string tabela)
            {
                  Command c = new Command(schema, tabela);

                  return c;
            }

            /// <summary>
            ///     ''' 
            ///     ''' </summary>
            ///     ''' <param name="tabela"></param>
            ///     ''' <param name="where">Parametro obrigatório para UPDATE E DELETE</param>
            ///     ''' <returns></returns>
            public static Command NewCommand(string schema, string tabela, string where)
            {
                  Command c = new Command(schema, tabela);
                  c.where = where;

                  return c;
            }

            public static SelectCommand NewSelect(string sql)
            {
                  SelectCommand c = new SelectCommand(sql);

                  return c;
            }


            public class CommandRow
            {
                  public string name;
                  public string type;
                  public string dataType;
                  public object value;

                  public CommandRow(string name, string type, string dataType, object value)
                  {
                        this.name = name;
                        this.type = type;
                        this.dataType = dataType;
                        this.value = value;
                  }

            }

            public class Command
            {
                  //public Command(string sql) : base()
                  //{
                  //    this.sql = sql;
                  //}
                  public Command(string schema, string tabela) : base()
                  {
                        this.schema = schema;
                        this.tabela = tabela;
                  }
                  public Command Where(string where)
                  {
                        this.where = where;

                        return this;
                  }
                  public Command Sql(string sql)
                  {
                        this.sql = sql;

                        return this;
                  }

                  public string? schema { get; set; }
                  public string? tabela { get; set; }
                  public string? sql { get; set; }

                  private string? _where;
                  public string? where
                  {
                        get
                        {
                              return _where;
                        }
                        set
                        {
                              if (value != null && strLen(value) > 0 && !$"{value}".Trim().ToLower().StartsWith("where"))
                                    value = "where " + value;
                              _where = value;
                        }
                  }
                  public MySqlTransaction? trans { get; set; }
                  private MySqlConnection? conn_;
                  public MySqlConnection? conn
                  {
                        get
                        {
                              return conn_;
                        }
                        set
                        {
                              if (!object.Equals(value, conn_))
                                    CloseConn(conn_);
                              conn_ = value;
                        }
                  }
                  private bool fecharConn = false;

                  private Dictionary<string, object?> cmp = new Dictionary<string, object?>();
                  private Dictionary<string, object?> cmpWhere = new Dictionary<string, object?>();

                  private bool __beguinTrans = false;
                  /// <summary>
                  ///         ''' Deverá chamar o metodo `Commit()` no final do processo
                  ///         ''' </summary>
                  ///         ''' <param name="StrConn"></param>
                  ///         ''' <returns></returns>
                  public Command BeginTransaction(string StrConn)
                  {
                        this.conn = new MySqlConnection(StrConn);
                        this.conn.Open();
                        this.trans = conn.BeginTransaction();

                        fecharConn = false;

                        __beguinTrans = true;

                        return this;
                  }

                  public void Commit()
                  {
                        if (__beguinTrans && !fecharConn && trans != null)
                        {
                              this.trans.Commit();
                              CloseConn(conn);
                        }
                  }

                  public void Rollback()
                  {
                        if (__beguinTrans && !fecharConn && trans != null)
                        {
                              this.trans.Rollback();
                              CloseConn(conn);
                        }
                  }

                  public Command SetConn(string StrConn, MySqlTransaction? trans)
                  {
                        if (trans is null)
                        {
                              return SetConn(StrConn);
                        }
                        else
                        {
                              return SetConn(trans);
                        }
                  }

                  public Command SetConn(MySqlTransaction? trans)
                  {
                        this.fecharConn = trans is null;
                        this.trans = trans;
                        this.conn = trans?.Connection;
                        return this;
                  }

                  public Command SetConn(MySqlConnection conn)
                  {
                        this.fecharConn = false;
                        this.conn = conn;
                        return this;
                  }

                  public Command SetConn(string StrConn)
                  {
                        this.conn = new MySqlConnection(StrConn);
                        this.conn.Open();
                        this.fecharConn = true;
                        return this;
                  }

                  public void RemoveNulls()
                  {
                        var l = cmp.ToArray();
                        foreach (var k in l)
                        {
                              if (object.Equals(k.Value, DBNull.Value) || k.Value == null || object.Equals(k.Value, ""))
                                    cmp.Remove(k.Key);
                        }
                  }

                  public Command Add(string key, object? valor)
                  {
                        if (where != null && where.Contains("@" + key))
                              this.cmpWhere[key] = valor;
                        else
                              cmp[key] = valor;

                        return this;
                  }

                  public Command AddParam(string key, object? valor)
                  {
                        return this.Add(key, valor);
                  }

                  public object? GetParam(string param)
                  {
                        if (this.ContainsKey(param))
                              return this.cmp[param];
                        return null;
                  }

                  public bool ContainsKey(string key)
                  {
                        return this.cmp.ContainsKey(key);
                  }

                  public bool RemoveParam(string key)
                  {
                        if (this.ContainsKey(key))
                              return this.cmp.Remove(key);
                        return false;
                  }

                  public object? GetValue(string key)
                  {
                        if (this.cmp.ContainsKey(key))
                              return this.cmp[key];
                        return null;
                  }

                  public int Insert()
                  {
                        if (conn == null && strConn != null)
                        {
                              SetConn(strConn);
                        }
                        var x = 0;
                        if (this.cmp.Count > 0)
                        {
                              MySqlCommand cmd = montaInsertSql(tabela!, ref this.cmp!, conn!, null, null);
                              cmd.Connection = conn;
                              cmd.Transaction = trans;

                              x = cmd.ExecuteNonQuery();
                              if (fecharConn)
                                    CloseConn(conn);
                        }
                        return x;
                  }

                  public Dictionary<string, CommandRow>? InsertAndGetColumns(string[] columns, string? pKeyName)
                  {
                        if (conn == null && strConn != null)
                        {
                              SetConn(strConn);
                        }
                        Dictionary<string, CommandRow>? ret = null;

                        if (this.cmp.Count > 0)
                        {
                              MySqlCommand cmd = montaInsertSql(tabela!, ref this.cmp!, conn!, columns, pKeyName);
                              cmd.Connection = conn;
                              cmd.Transaction = trans;

                              MySqlDataReader data = cmd.ExecuteReader();
                              try
                              {
                                    if (data.Read())
                                    {
                                          ret = new Dictionary<string, CommandRow>();
                                          for (int i = 0; i < data.FieldCount; i++)
                                          {
                                                ret[data.GetName(i)] = new CommandRow(data.GetName(i), data.GetFieldType(i).ToString(), data.GetDataTypeName(i), data.GetValue(i));
                                          }
                                    }
                              }
                              catch (Exception ex)
                              {
                                    throw ex;
                              }
                              finally
                              {
                                    data.Close();
                              }

                              if (fecharConn)
                                    CloseConn(conn);
                        }
                        return ret;
                  }

                  public int Update()
                  {
                        if (conn == null && strConn != null)
                        {
                              SetConn(strConn);
                        }
                        var x = 0;
                        if (this.cmp.Count > 0)
                        {
                              MySqlCommand cmd = montaUpdateSql(tabela!, ref this.cmp!, conn!, where!);

                              foreach (string k in this.cmpWhere.Keys)
                                    cmd.Parameters.AddWithValue($"@{k}", cmpWhere[k]);

                              cmd.Connection = conn;
                              cmd.Transaction = trans;

                              x = cmd.ExecuteNonQuery();
                              if (fecharConn)
                                    CloseConn(conn);
                        }
                        return x;
                  }

                  public int Delete()
                  {
                        if (conn == null && strConn != null)
                        {
                              SetConn(strConn);
                        }
                        var x = 0;
                        if (this.cmpWhere.Count > 0)
                        {
                              MySqlCommand cmd = new MySqlCommand("delete from " + tabela + " " + where);
                              foreach (string k in cmpWhere.Keys)
                                    cmd.Parameters.AddWithValue($"@{k}", cmpWhere[k]);
                              cmd.Connection = conn;
                              cmd.Transaction = trans;

                              x = cmd.ExecuteNonQuery();
                              if (fecharConn)
                                    CloseConn(conn);
                        }
                        return x;
                  }

            }
            public class SelectCommand : IDisposable
            {
                  public SelectCommand(string sql) : base()
                  {
                        this.sql = sql;
                  }

                  public string sql { get; set; }

                  public MySqlTransaction? trans { get; set; }
                  private MySqlConnection? conn_;
                  public MySqlConnection? conn
                  {
                        get
                        {
                              return conn_;
                        }
                        set
                        {
                              if (!object.Equals(value, conn_))
                                    CloseConn(conn_);
                              conn_ = value;
                        }
                  }
                  private bool fecharConn = false;

                  private Dictionary<string, object?> cmpWhere = new Dictionary<string, object?>();
                  public int cmpWhereCount
                  {
                        get
                        {
                              return cmpWhere.Count;
                        }
                  }

                  private string? where;
                  private string? orderBy;
                  private int Limit;
                  public SelectCommand setWhere(string where)
                  {
                        this.where = where;
                        return this;
                  }
                  public SelectCommand SetOrder(string? order)
                  {
                        this.orderBy = order;
                        return this;
                  }
                  public SelectCommand setLimit(int limit)
                  {
                        this.Limit = limit;
                        return this;
                  }
                  public SelectCommand SetConn(string StrConn, MySqlTransaction? trans)
                  {
                        if (trans is null)
                        {
                              return SetConn(StrConn);
                        }
                        else
                        {
                              return SetConn(trans);
                        }
                  }
                  public SelectCommand SetConn(MySqlTransaction trans)
                  {
                        this.fecharConn = false;
                        this.trans = trans;
                        if (trans != null)
                              this.conn = trans.Connection;
                        return this;
                  }

                  public SelectCommand SetConn(MySqlConnection conn)
                  {
                        this.conn = conn;
                        this.fecharConn = false;
                        return this;
                  }

                  public SelectCommand SetConn(string StrConn)
                  {
                        this.conn = new MySqlConnection(StrConn);
                        this.conn.Open();
                        fecharConn = true;
                        return this;
                  }

                  public SelectCommand Add(string key, object? valor)
                  {
                        this.cmpWhere[key] = valor;

                        return this;
                  }

                  public SelectCommand AddParam(string key, object? valor)
                  {
                        return this.Add(key, valor);
                  }

                  public bool ContainsKey(string key)
                  {
                        return this.cmpWhere.ContainsKey(key);
                  }

                  public T? Param<T>(string key)
                  {
                        if (this.cmpWhere.ContainsKey(key))
                              return (T?)this.cmpWhere[key];
                        return default;
                  }
                  /// <summary>
                  ///         ''' Será executado ´Select(False)´ caso não tenha sido executando anteriormente
                  ///         ''' </summary>
                  ///         ''' <returns></returns>
                  public object? Value(string ColumnName)
                  {
                        if (!Select_executado)
                              this.Select(true);
                        if (Debugger.IsAttached)
                        {
                              if (!this.tb.Columns.Contains(ColumnName))
                                    Debugger.Break();
                        }
                        if (this.current != null && this.tb.Columns.Contains(ColumnName))
                              return this.current[ColumnName];
                        return null;
                  }
                  public string? String(string ColumnName)
                  {
                        object? v = this.Value(ColumnName);
                        if (v != DBNull.Value && v != null)
                              return CStr(v);
                        return default;
                  }
                  public string String(string ColumnName, string padrao)
                  {
                        object? v = this.Value(ColumnName);
                        if (v != DBNull.Value && v != null)
                              return CStr(v, padrao)!;
                        return padrao;
                  }
                  public int Int(string ColumnName)
                  {
                        object? v = this.Value(ColumnName);
                        if (v != DBNull.Value && v != null)
                              return CInt(v, 0) ?? 0;
                        return default;
                  }
                  public int? Int(string ColumnName, int? padrao)
                  {
                        object? v = this.Value(ColumnName);
                        if (v != DBNull.Value && v != null)
                              return CInt(v, padrao);
                        return default;
                  }
                  public long Lng(string ColumnName)
                  {
                        object? v = this.Value(ColumnName);
                        if (v != DBNull.Value && v != null)
                              return CLong(v, 0) ?? 0;
                        return default;
                  }
                  public long? Lng(string ColumnName, long? padrao)
                  {
                        object? v = this.Value(ColumnName);
                        if (v != DBNull.Value && v != null)
                              return CLong(v, padrao);
                        return default;
                  }
                  public decimal Decimal(string ColumnName)
                  {
                        object? v = this.Value(ColumnName);
                        if (v != DBNull.Value && v != null)
                              return CDec(v);
                        return default;
                  }
                  public bool Boolean(string ColumnName)
                  {
                        object? v = this.Value(ColumnName);
                        if (v != DBNull.Value && v != null)
                              return CBool(v);
                        return default;
                  }
                  public DateTime? Date(string ColumnName)
                  {
                        object? v = this.Value(ColumnName);
                        if (v != DBNull.Value && v != null)
                              return CDate(v);
                        return default;
                  }

                  /// <summary>
                  ///         ''' Será executado ´Select(False)´ caso não tenha sido executando anteriormente
                  ///         ''' </summary>
                  ///         ''' <returns></returns>
                  public int Count
                  {
                        get
                        {
                              if (!Select_executado)
                                    this.Select(false);
                              return tb.Rows.Count;
                        }
                  }

                  public DataTable tb = new DataTable();
                  /// <summary>
                  /// Executa um select e navega para primeiro registro se existir
                  /// </summary>
                  /// <returns></returns>
                  public SelectCommand Select()
                  {
                        return Select(true);
                  }
                  private bool Select_executado = false;
                  public SelectCommand Select(bool primeiroRegistro)
                  {
                        if (conn == null && strConn != null)
                        {
                              SetConn(strConn);
                        }

                        tb.Clear();

                        if (conn == null && trans == null)
                        {
                              throw new InvalidOperationException("Não foi informado uma conexão!");
                        }
                        else if (conn != null && conn.State != ConnectionState.Open)
                              conn.Open();

                        using (MySqlCommand cmd = new MySqlCommand($"{sql} {where} {orderBy} {(Limit > 0 ? $"LIMIT {Limit}" : "")}", conn))
                        {
                              cmd.Transaction = trans;

                              foreach (string k in this.cmpWhere.Keys)
                                    cmd.Parameters.AddWithValue($"@{k}", this.cmpWhere[k]);

                              MySqlDataAdapter ad = new MySqlDataAdapter(cmd);
                              ad.Fill(tb);
                        }

                        Select_executado = true;
                        if (fecharConn)
                              CloseConn(conn);

                        if (primeiroRegistro)
                              this.Primeiro();

                        return this;
                  }

                  private int _index = -1;
                  public int index
                  {
                        get
                        {
                              return _index;
                        }
                  }

                  private DataRow? current = null;
                  /// <summary>
                  ///         ''' Será executado ´Select(False)´ caso não tenha sido executando anteriormente
                  ///         ''' </summary>
                  ///         ''' <returns></returns>
                  public bool Primeiro()
                  {
                        if (!Select_executado)
                              Select(false);
                        current = null;
                        if (this.tb.Rows.Count > 0)
                        {
                              _index = 0;
                              current = this.tb.Rows[_index];
                        }

                        return current != null;
                  }
                  /// <summary>
                  ///         ''' Será executado ´Select(False)´ caso não tenha sido executando anteriormente
                  ///         ''' </summary>
                  ///         ''' <returns></returns>
                  public bool Proximo()
                  {
                        if (!Select_executado)
                              Select(false);
                        int i = _index;
                        current = null;
                        _index = -1;
                        if (this.tb.Rows.Count > i + 1)
                        {
                              _index = i + 1;
                              current = this.tb.Rows[_index];
                        }

                        return current != null;
                  }
                  /// <summary>
                  ///         ''' Será executado ´Select(False)´ caso não tenha sido executando anteriormente
                  ///         ''' </summary>
                  ///         ''' <returns></returns>
                  public bool Anterior()
                  {
                        if (!Select_executado)
                              Select(false);
                        int i = _index;
                        current = null;
                        _index = -1;
                        if (this.tb.Rows.Count > 0 && i - 1 >= 0)
                        {
                              _index = i - 1;
                              current = this.tb.Rows[_index];
                        }

                        return current != null;
                  }
                  /// <summary>
                  ///         ''' Será executado ´Select(False)´ caso não tenha sido executando anteriormente
                  ///         ''' </summary>
                  ///         ''' <returns></returns>
                  public bool Ultimo()
                  {
                        if (!Select_executado)
                              Select(false);
                        current = null;
                        _index = -1;
                        if (this.tb.Rows.Count > 0)
                        {
                              _index = this.tb.Rows.Count - 1;
                              current = this.tb.Rows[_index];
                        }

                        return current != null;
                  }

                  private bool disposedValue; // Para detectar chamadas redundantes

                  // IDisposable
                  protected virtual void Dispose(bool disposing)
                  {
                        if (!this.disposedValue)
                        {
                              if (disposing)
                              {
                              }

                              this.cmpWhere.Clear();
                              if (fecharConn)
                                    this.conn?.Close();
                              if (fecharConn)
                                    this.conn?.Dispose();
                              this.current = null;
                              this.tb.Clear();
                              this._index = -1;
                        }
                        this.disposedValue = true;
                  }

                  ~SelectCommand()
                  {
                        // Não altere este código. Coloque o código de limpeza em Dispose(disposing As Boolean) acima.
                        Dispose(false);
                        //base.Finalize();
                  }

                  // Código adicionado pelo Visual Basic para implementar corretamente o padrão descartável.
                  public void Dispose()
                  {
                        // Não altere este código. Coloque o código de limpeza em Dispose(disposing As Boolean) acima.
                        Dispose(true);
                        GC.SuppressFinalize(this);
                  }
            }



            public static MySqlCommand montaInsertSql(string tabela, ref Dictionary<string, object> campos, MySqlConnection conn, string[]? outputColumns, string? pKeyName)
            {
                  MySqlCommand cmd = new MySqlCommand();
                  string virg = "";
                  string cmp = "";
                  string vals = "";
                  int i = 0;
                  Dictionary<string, object> c = new Dictionary<string, object>();
                  foreach (string k in campos.Keys)
                        c[k] = campos[k];
                  foreach (string k in c.Keys)
                  {
                        if (i.Equals(0))
                              virg = "";
                        else
                              virg = ", ";

                        cmp += $"{virg}`{k}`";

                        if (object.Equals(c[k], DBNull.Value) || (c[k] == null))
                        {
                              vals += virg + "@" + k;
                              campos[k] = DBNull.Value;
                              cmd.Parameters.AddWithValue($"@{k}", campos[k]);
                        }
                        else if (c[k].GetType() == typeof(string))
                        {
                              vals += virg + "@" + k;
                              campos[k] = ((string.IsNullOrEmpty(c[k] as string)) ? DBNull.Value : c[k]);
                              cmd.Parameters.AddWithValue($"@{k}", campos[k]);
                        }
                        else if (c[k].GetType() == typeof(double) || c[k].GetType() == typeof(float) || c[k].GetType() == typeof(decimal))
                        {
                              vals += virg + "@" + k;
                              campos[k] = CDec(c[k]);
                              cmd.Parameters.AddWithValue($"@{k}", campos[k]);
                        }
                        else
                        {
                              vals += virg + "@" + k;
                              cmd.Parameters.AddWithValue($"@{k}", campos[k]);
                        }

                        i += 1;
                  }
                  // foreach
                  //string sql = "INSERT INTO " + tabela + " ( " + cmp + " ) OUTPUT Inserted.ID VALUES ( " + vals + " ) ";
                  string sql = "INSERT INTO " + tabela + " ( " + cmp + " )";

                  sql += " VALUES ( " + vals + " ) ;";

                  if (outputColumns != null && outputColumns.Length > 0)
                  {
                        sql += $" SELECT {String.Join(", ", outputColumns.Select(c => $"`{c}`").ToArray())} from {tabela} WHERE `{pKeyName}` = LAST_INSERT_ID()";
                  }

                  cmd.CommandText = sql;
                  cmd.Connection = conn;



                  return cmd;
            }

            public static MySqlCommand montaUpdateSql(string tabela, ref Dictionary<string, object> campos, MySqlConnection conn, string par)
            {
                  if (string.IsNullOrEmpty(par))
                        throw new Exception("O parâmetro 'par' em 'montaUpdateSql()' não deve ser nulo.");
                  MySqlCommand cmd = new MySqlCommand();
                  string virg;
                  string sql = "UPDATE " + tabela + " SET ";

                  int i = 0;
                  Dictionary<string, object> c = new Dictionary<string, object>();
                  foreach (string k in campos.Keys)
                        c[k] = campos[k];
                  foreach (string k in c.Keys)
                  {
                        if (i.Equals(0))
                              virg = "";
                        else
                              virg = ", ";

                        sql += $"{virg}`{k}` = @{k}";

                        if (Equals(c[k], DBNull.Value) || (c[k] == null))
                        {
                              campos[k] = DBNull.Value;
                              cmd.Parameters.AddWithValue($"@{k}", campos[k]);
                        }
                        else if (c[k].GetType() == typeof(string))
                        {
                              campos[k] = ((string.IsNullOrEmpty(c[k] as string)) ? DBNull.Value : c[k]);
                              cmd.Parameters.AddWithValue($"@{k}", campos[k]);
                        }
                        else if (c[k].GetType() == typeof(double) || c[k].GetType() == typeof(float) || c[k].GetType() == typeof(decimal))
                        {
                              campos[k] = CDec(c[k]);
                              cmd.Parameters.AddWithValue($"@{k}", campos[k]);
                        }
                        else
                              cmd.Parameters.AddWithValue($"@{k}", campos[k]);
                        i += 1;
                  }
                  cmd.CommandText = sql + " " + par;
                  cmd.Connection = conn;

                  return cmd;
            }
      }
}