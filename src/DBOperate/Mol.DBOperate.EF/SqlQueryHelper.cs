using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Manphi.DBOperate.EF
{
    public static class SqlQueryHelper
    {
        public static List<T> SqlQueryToDataTable<T>(this DbContext db, string sql, params object[] parameters) where T : new()
        {
            //注意：不要对GetDbConnection获取到的conn进行using或者调用Dispose，否则DbContext后续不能再进行使用了，会抛异常
            var conn = db.Database.GetDbConnection();
            try
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    var readerFields = new List<string>();
                    command.CommandText = sql;
                    command.Parameters.AddRange(parameters);
                    var propts = typeof(T).GetProperties();
                    var rtnList = new List<T>();
                    T model;
                    object val;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            for (var i = 0; i < reader.FieldCount; i++)
                            {
                                readerFields.Add(reader.GetName(i).ToUpper());
                            }

                            model = new T();
                            foreach (var l in propts)
                            {

                                if (readerFields.IndexOf(l.Name.ToUpper()) > -1)
                                {
                                    val = reader[l.Name];
                                    if (val == DBNull.Value)
                                    {
                                        l.SetValue(model, null);
                                    }
                                    else
                                    {
                                        l.SetValue(model, val);
                                    }
                                }
                                else
                                {
                                    l.SetValue(model, null);
                                }
                            }
                            rtnList.Add(model);
                        }
                    }
                    return rtnList;
                }
            }
            finally
            {
                conn.Close();
            }
        }
        public static List<T> SqlQueryToDataTableForTrans<T>(this DbContext db, string sql, params object[] parameters) where T : new()
        {
            //注意：不要对GetDbConnection获取到的conn进行using或者调用Dispose，否则DbContext后续不能再进行使用了，会抛异常
            var conn = db.Database.GetDbConnection();
            using (var command = conn.CreateCommand())
            {
                var readerFields = new List<string>();
                command.CommandText = sql;
                command.Parameters.AddRange(parameters);
                var propts = typeof(T).GetProperties();
                var rtnList = new List<T>();
                T model;
                object val;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            readerFields.Add(reader.GetName(i).ToUpper());
                        }

                        model = new T();
                        foreach (var l in propts)
                        {

                            if (readerFields.IndexOf(l.Name.ToUpper()) > -1)
                            {
                                val = reader[l.Name];
                                if (val == DBNull.Value)
                                {
                                    l.SetValue(model, null);
                                }
                                else
                                {
                                    l.SetValue(model, val);
                                }
                            }
                            else
                            {
                                l.SetValue(model, null);
                            }
                        }
                        rtnList.Add(model);
                    }
                }
                return rtnList;
            }
        }

        public static DataTable SqlQueryToDataTable(this DbContext db, string sql, params object[] parameters)
        {
            //注意：不要对GetDbConnection获取到的conn进行using或者调用Dispose，否则DbContext后续不能再进行使用了，会抛异常
            var conn = db.Database.GetDbConnection();
            try
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = sql;
                    command.Parameters.AddRange(parameters);
                    var dataReader = command.ExecuteReader();
                    var datatable = new DataTable();
                    try
                    {
                        for (var i = 0; i < dataReader.FieldCount; i++)
                        {
                            var myDataColumn = new DataColumn
                            {
                                DataType = dataReader.GetFieldType(i),
                                ColumnName = dataReader.GetName(i)
                            };
                            datatable.Columns.Add(myDataColumn);
                        }
                        while (dataReader.Read())
                        {
                            var myDataRow = datatable.NewRow();
                            for (var i = 0; i < dataReader.FieldCount; i++)
                            {
                                myDataRow[i] = dataReader[i];
                            }
                            datatable.Rows.Add(myDataRow);
                            myDataRow = null;
                        }
                        dataReader.Close();
                        return datatable;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message, ex);
                    }
                }
            }
            finally
            {
                conn.Close();
            }
        }
        public static DataTable ConvertDataReaderToDataTable(DbDataReader dataReader)
        {
            var datatable = new DataTable();
            try
            {
                for (var i = 0; i < dataReader.FieldCount; i++)
                {
                    var myDataColumn = new DataColumn
                    {
                        DataType = dataReader.GetFieldType(i),
                        ColumnName = dataReader.GetName(i)
                    };
                    datatable.Columns.Add(myDataColumn);
                }
                while (dataReader.Read())
                {
                    var myDataRow = datatable.NewRow();
                    for (var i = 0; i < dataReader.FieldCount; i++)
                    {
                        myDataRow[i] = dataReader[i];
                    }
                    datatable.Rows.Add(myDataRow);
                    myDataRow = null;
                }
                dataReader.Close();
                return datatable;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// 将DataReader转换为泛型集合
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="dr">DataReader</param>
        /// <returns></returns>
        private static List<T> DataReaderToList<T>(IDataReader dr) where T : class
        {
            List<T> list = new List<T>();
            Type t = typeof(T);
            PropertyInfo[] properties = t.GetProperties();
            while (dr.Read())
            {
                dynamic o = Activator.CreateInstance(t) as T;
                foreach (PropertyInfo p in properties)
                {
                    if (!Convert.IsDBNull(dr[p.Name]))
                    {
                        p.SetValue(o, dr[p.Name].ToString(), null);
                    }
                }
                T e = o as T;
                list.Add(e);
            }
            return list;
        }
    }
}
