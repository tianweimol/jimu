using Microsoft.EntityFrameworkCore;
using Manphi.DBOperate.Model;
using System.Collections.Generic;
using System.Data;

namespace Manphi.DbOperate.EF
{
    public interface IBaseDbContext
    {
        /// <summary>
        /// Get DbSet
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>DbSet</returns>
        DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity;

        /// <summary>
        /// Save changes
        /// </summary>
        /// <returns>Result</returns>
        int SaveChanges();

        /// <summary>
        /// Execute stores procedure and load a list of entities at the end
        /// </summary>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>DataTable</returns>
        DataTable ExecuteStoredProcedureList(string commandText, params object[] parameters);

        /// <summary>
        /// 返回对应的实体
        /// </summary>
        /// <typeparam name="T">泛型实体</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数：根据数据库不用，传入的类型不同（sqlserver：SqlParameter[],oracle:OracleParameter[],mysql:MySqlParameter[]）</param>
        /// <returns>list<T></returns>
        List<T> SqlQuery<T>(string sql, params object[] parameters) where T : new();
        /// <summary>
        /// 返回dataTable
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数：根据数据库不用，传入的类型不同（sqlserver：SqlParameter[],oracle:OracleParameter[],mysql:MySqlParameter[]）</param>
        /// <returns>DataTable</returns>
        DataTable SqlQuery(string sql, params object[] parameters);

        List<T> SqlQueryForTrans<T>(string sql, params object[] parameters) where T : new();
    }
}
