using Manphi.DBOperate.EF;
using Manphi.DBOperate.Mapping;
using Microsoft.EntityFrameworkCore;
using Mol.DBOperate.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace Mol.DBOperate.EF
{
    // TODO: Context应该是不能做成泛型的，要不然mapping在modelcreating中就不好写了。
    public class BaseDBContext : DbContext, IBaseDbContext
    {
        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="nameOrConnectionString">Connecting string</param>
        public BaseDBContext(DbContextOptions nameOrConnectionString) : base(nameOrConnectionString)
        {

        }
        #endregion

        #region Utilities

        /// <summary>
        /// On model creating
        /// </summary>
        /// <param name="modelBuilder">Model builder</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var typesToRegister = Assembly.GetAssembly(typeof(ManphiEntityTypeConfiguration<>)).GetTypes()
                .Where(type => !string.IsNullOrEmpty(type.Namespace))
                .Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(ManphiEntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.ApplyConfiguration(configurationInstance);
            }
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Attach an entity to the context or return an already attached entity (if it was already attached)
        /// </summary>
        /// <typeparam name="TEntity">TEntity</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>Attached entity</returns>
        protected virtual TEntity AttachEntityToContext<TEntity>(TEntity entity) where TEntity : BaseEntity, new()
        {
            //little hack here until Entity Framework really supports stored procedures
            //otherwise, navigation properties of loaded entities are not loaded until an entity is attached to the context
            var alreadyAttached = Set<TEntity>().Local.FirstOrDefault(x => x.Id == entity.Id);
            if (alreadyAttached == null)
            {
                //attach new entity
                Set<TEntity>().Attach(entity);
                return entity;
            }

            //entity is already loaded
            return alreadyAttached;
        }

        #endregion

        /// <summary>
        /// Get DbSet
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>DbSet</returns>
        public new DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }

        /// <summary>
        /// Execute stores procedure and load a list of entities at the end
        /// </summary>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>Entities</returns>
        public DataTable ExecuteStoredProcedureList(string commandText, params object[] parameters)
        {
            //add parameters to command
            if (parameters != null && parameters.Length > 0)
            {
                for (var i = 0; i <= parameters.Length - 1; i++)
                {
                    var p = parameters[i] as DbParameter;
                    if (p == null)
                        throw new Exception("Not support parameter type");

                    commandText += i == 0 ? " " : ", ";

                    commandText += "@" + p.ParameterName;
                    if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output)
                    {
                        //output parameter
                        commandText += " output";
                    }
                }
            }
            var result = this.SqlQueryToDataTable(commandText, parameters);
            return result;
        }

        /// <summary>
        /// Creates a raw SQL query that will return elements of the given generic type.  The type can be any type that has properties that match the names of the columns returned from the query, or can be a simple primitive type. The type does not have to be an entity type. The results of this query are never tracked by the context even if the type of object returned is an entity type.
        /// </summary>
        /// <param name="sql">The SQL query string.</param>
        /// <param name="parameters">The parameters to apply to the SQL query string.</param>
        /// <returns>Result</returns>
        //public DataTable SqlQuery(string sql, params object[] parameters) => this.SqlQueryToDataTable(sql, parameters);

        public List<T> SqlQuery<T>(string sql, params object[] parameters) where T : new() => this.SqlQueryToDataTable<T>(sql, parameters);
        public DataTable SqlQuery(string sql, params object[] parameters) => this.SqlQueryToDataTable(sql, parameters);
        public List<T> SqlQueryForTrans<T>(string sql, params object[] parameters) where T : new() => this.SqlQueryToDataTableForTrans<T>(sql, parameters);

    }
}
