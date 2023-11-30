using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace util
{
      public static class funcoes
      {
            public static int strLen(string? str)
            {
                  if (str != null)
                  {
                        return str.Length;
                  }
                  return 0;
            }
            public static void CloseConn(MySqlConnection? conn)
            {
                  if (conn != null && conn.State == ConnectionState.Open)
                        conn.Close();
            }
            public static string CStr(this decimal valor, string separadorDecimais = ",", int decimais = 2, bool nullSeZero = false)
            {
                  string v = null;

                  if (nullSeZero && valor == 0.0M)
                  {
                        v = null;
                  }
                  else
                  {
                        v = valor.ToString($"N{decimais}");
                        if (separadorDecimais == ".")
                        {
                              v = v.Replace(".", "").Replace(",", ".");
                        }
                  }

                  return v;
            }

            public static string? CStr(object? obj, string? padrao = null, int MaxLength = 0)
            {
                  string? v = padrao;
                  if (obj != null && !Equals(obj, DBNull.Value))
                  {
                        v = Convert.ToString(obj);
                        if (IsNullOrEmpty(v))
                        {
                              v = padrao;
                        }
                  }
                  if (MaxLength > 0 && v != null && v?.Length > MaxLength)
                  {
                        v = v.Substring(0, MaxLength);
                  }

                  return v;
            }
            public static bool IsNullOrEmpty(object valor)
            {
                  bool v = false;
                  if (valor == null)
                  {
                        v = true;
                  }
                  else if (Equals(valor, DBNull.Value))
                  {
                        v = true;
                  }
                  else if (valor.GetType() == typeof(string))
                  {
                        v = (valor as string == "");
                  }

                  return v;
            }
            public static decimal CDec(object? valor, decimal padrao = (decimal)0.0)
            {
                  decimal v = padrao;
                  if (!Equals(valor, DBNull.Value) && valor != null)
                  {
                        if (valor.GetType() == typeof(decimal))
                        {
                              return (decimal)valor;
                        }

                        string s = valor.ToString();
                        if (s.Contains(".") && s.Contains(","))
                        {
                              if (s.IndexOf(".") > s.IndexOf(","))
                              {
                                    s = s.Replace(",", "").Replace(".", ",");
                              }
                              else
                              {
                                    s = s.Replace(".", "");
                              }

                        }
                        else if (s.Contains("."))
                        {
                              s = s.Replace(".", ",");
                        }
                        try
                        {
                              decimal.TryParse(s, out v);
                        }
                        catch { }
                  }

                  return v;
            }
            public static int CInt(object? valor)
            {
                  return CInt(valor, 0) ?? 0;
            }
            public static int? CInt(object? valor, int? padrao = 0)
            {
                  int? v = padrao;
                  if (!Equals(valor, DBNull.Value) && valor != null)
                  {
                        string s = CStr(valor, "");
                        if (s.Contains(".") && s.Contains(","))
                        {
                              if (s.IndexOf(".") > s.IndexOf(","))
                              {
                                    s = s.Replace(",", "").Replace(".", ",");
                              }
                              else
                              {
                                    s = s.Replace(".", "");
                              }

                        }
                        else if (s.Contains("."))
                        {
                              s = s.Replace(".", ",");
                        }
                        if (s.Contains(".") || s.Contains(","))
                        {
                              valor = CDec(s);
                        }
                        else
                        {
                              valor = s;
                        }
                        try
                        {
                              v = Convert.ToInt32(valor);
                        }
                        catch { }
                  }

                  return v;
            }
            public static long? CLong(object? valor, long? padrao = 0)
            {
                  long? v = padrao;
                  if (!Equals(valor, DBNull.Value) && valor != null)
                  {
                        string s = CStr(valor, "");
                        if (s.Contains(".") && s.Contains(","))
                        {
                              if (s.IndexOf(".") > s.IndexOf(","))
                              {
                                    s = s.Replace(",", "").Replace(".", ",");
                              }
                              else
                              {
                                    s = s.Replace(".", "");
                              }

                        }
                        else if (s.Contains("."))
                        {
                              s = s.Replace(".", ",");
                        }
                        if (s.Contains(".") || s.Contains(","))
                        {
                              valor = CDec(s);
                        }
                        else
                        {
                              valor = s;
                        }
                        try
                        {
                              v = Convert.ToInt64(valor);
                        }
                        catch { }
                  }

                  return v;
            }
            public static DateTime? CDate(object? valor, DateTime? padrao = null)
            {
                  DateTime? v = padrao;
                  if (!Equals(valor, DBNull.Value) && valor != null)
                  {
                        try
                        {
                              v = Convert.ToDateTime(valor);
                        }
                        catch { }
                  }

                  return v;
            }
            public static bool CBool(object? valor, bool padrao = false)
            {
                  if (Equals(valor, DBNull.Value) || valor == null)
                  {
                        return padrao;
                  }
                  else if (true.Equals(valor))
                  {
                        return true;
                  }
                  else if (false.Equals(valor))
                  {
                        return false;
                  }
                  else if ((IsNumeric(valor) && CInt(valor) == 1))
                  {
                        return true;
                  }

                  if (bool.TryParse(CStr(valor), out padrao))
                  {
                        return bool.Parse(CStr(valor));
                  }
                  else if ((IsNumeric(valor) && CInt(valor) == 0))
                  {
                        return false;
                  }
                  return padrao;
            }
            public static bool CBool(object? valor)
            {
                  bool v = false;
                  if (!Equals(valor, DBNull.Value) && valor != null)
                  {
                        try
                        {
                              if (valor.GetType() == typeof(string))
                              {
                                    if ((valor as string).ToLower().Equals("false")
                                        || (valor as string).ToLower().Equals("no")
                                        || (valor as string).ToLower().Equals("nao")
                                        || (valor as string).ToLower().Equals("não"))
                                    {
                                          valor = false;
                                    }
                                    else if ((valor as string).ToLower().Equals("true")
                                       || (valor as string).ToLower().Equals("yes")
                                       || (valor as string).ToLower().Equals("sim"))
                                    {
                                          valor = true;
                                    }
                                    else
                                    {
                                          valor = CInt(valor);
                                    }
                              }
                              else if (valor.GetType() == typeof(float))
                              {
                                    valor = CInt(valor);
                              }

                              v = Convert.ToBoolean(valor);
                        }
                        catch
                        {
                              v = false;
                        }
                  }

                  return v;
            }
            public static bool IsNumeric(object? valor)
            {
                  bool x = false;

                  if (valor == null || valor == DBNull.Value)
                  {
                        x = false;
                  }
                  else if (new Type[] { typeof(bool)
                            , typeof(byte)
                            , typeof(decimal)
                            , typeof(double)
                            , typeof(int)
                            , typeof(long)
                            , typeof(ulong)
                            , typeof(sbyte)
                            , typeof(short)
                            , typeof(Single)
                            , typeof(uint)
                            , typeof(ushort)}.Contains(valor.GetType()))
                  {
                        x = true;
                  }
                  else if (new Type[] { typeof(DateTime)
                            , typeof(object)}.Contains(valor.GetType()))
                  {
                        x = false;
                  }
                  else
                  {
                        if (valor.GetType() == typeof(char))
                        {
                              valor = Convert.ToString((char)valor);
                        }
                        try
                        {
                              Convert.ToDouble(valor);
                              x = true;
                        }
                        catch
                        {
                              x = false;
                        }
                  }


                  return x;
            }

      }
}
