using Jimu.Server.ORM.Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Mol.DBOperate.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace Mol.DBOperate.EF
{
    public class BaseContext<T> : DbContext, IBaseContext<T> where T : BaseEntity
    {
        private readonly DbContext _currentContext;
        private DbSet<T> _entities;
        private readonly string _connectionString;
        #region cort
        public BaseContext(DbContextOptions option, IOptions<DapperOptions> dboptions) : base(option)
        {
            _connectionString = dboptions.Value.ConnectionString;
            _currentContext = new DbContext(option);
        }

        #endregion cort
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context">上下文对象</param>
        public BaseContext(DbContextOptions<BaseContext<T>> options, IOptions<DapperOptions> dboptions) : base(options)
        {
            // 看看连接串放哪了
            _connectionString = dboptions.Value.ConnectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { }

        /// <summary>
        /// Attach an entity to the context or return an already attached entity (if it was already attached)
        /// </summary>
        /// <typeparam name="TEntity">TEntity</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>Attached entity</returns>
        protected virtual TEntity AttachEntityToContext<TEntity>(TEntity entity) where TEntity : BaseEntity, new()
        {
            var alreadyAttached = Set<TEntity>().Local.FirstOrDefault(x => x.Id == entity.Id);
            if (alreadyAttached == null)
            {
                Set<TEntity>().Attach(entity);
                return entity;
            }
            return alreadyAttached;
        }



        /// <summary>
        /// Get DbSet
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>DbSet</returns>
        public new DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }


        public DbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = _currentContext.Set<T>();
                return _entities;
            }
        }
        /// <summary>
        /// 获得IQueryable对象
        /// </summary>
        public virtual IQueryable<T> Table
        {
            get
            {
                return this.Entities;
            }
        }


        /// <summary>
        /// No tracking 实体不被EF context跟踪，仅用于只读操作
        /// </summary>
        public virtual IQueryable<T> TableNoTracking
        {
            get
            {
                return this.Entities.AsNoTracking();
            }
        }

        public IList<T> GetListAll()
        {
            return _currentContext.Set<T>().OrderByDescending(t => t.Id).ToList();
        }

        public IQueryable<T> GetListByPage<TKey>(Expression<Func<T, T>> selectLambda, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda, int pageSize, int pageIndex, out int total, bool isAsc = true)
        {
            total = _currentContext.Set<T>().Where(whereLambda).Count();
            var result = _currentContext.Set<T>().Where(whereLambda);
            if (isAsc)
            {
                result = result.OrderBy(orderLambda);
            }
            else
            {
                result = result.OrderByDescending(orderLambda);
            }

            return result.Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(selectLambda).AsQueryable<T>();
        }

        public IQueryable<dynamic> GetListByPage<TKey>(Expression<Func<T, dynamic>> selectLambda, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda, int pageSize, int pageIndex, out int total, bool isAsc = true)
        {
            total = _currentContext.Set<T>().Where(whereLambda).Count();
            var result = _currentContext.Set<T>().Where(whereLambda);
            if (isAsc)
            {
                result = result.OrderBy(orderLambda);
            }
            else
            {
                result = result.OrderByDescending(orderLambda);
            }
            return result.Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(selectLambda).AsQueryable<dynamic>();
        }

        public IQueryable<T> GetListByPageBase<TKey>(Expression<Func<T, T>> selectLambda, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda, bool isAsc = true)
        {
            var result = _currentContext.Set<T>().Where(whereLambda);
            if (isAsc)
            {
                result = result.OrderBy(orderLambda);
            }
            else
            {
                result = result.OrderByDescending(orderLambda);
            }
            return result.Select(selectLambda).AsQueryable<T>();
        }
        public IQueryable<T> GetListByPageBase<TKey>(Expression<Func<T, T>> selectLambda, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda, Expression<Func<T, TKey>> thenByLambda, Expression<Func<T, TKey>> thenByDescLambda, bool isAsc = true, bool thenByFirs = true)
        {
            var result = _currentContext.Set<T>().Where(whereLambda);
            if (isAsc)
            {
                result = result.OrderBy(orderLambda);
            }
            else
            {
                result = result.OrderByDescending(orderLambda);
            }
            if (thenByFirs)
            {
                result = (result as IOrderedQueryable<T>).ThenBy(thenByLambda).ThenByDescending(thenByDescLambda);
            }
            else
            {
                result = (result as IOrderedQueryable<T>).ThenByDescending(thenByDescLambda).ThenBy(thenByLambda);
            }
            return result.Select(selectLambda).AsQueryable<T>();
        }

        public IQueryable<dynamic> GetListByPageBase<TKey>(Expression<Func<T, dynamic>> selectLambda, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda, bool isAsc = true)
        {
            var result = _currentContext.Set<T>().Where(whereLambda);
            if (isAsc)
            {
                result = result.OrderBy(orderLambda);
            }
            else
            {
                result = result.OrderByDescending(orderLambda);
            }
            return result.Select(selectLambda).AsQueryable<dynamic>();
        }

        public int Add(IEnumerable<T> input)
        {
            foreach (T model in input)
            {
                ((DbContext)_currentContext).Entry<T>(model).State = EntityState.Added;
            }
            return _currentContext.SaveChanges();
        }
        /// <summary>
        /// 删除一个ID对应的实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            T delModel = GetModel(id);
            //delModel.IsDeleted = true;
            this.Entities.Remove(delModel);
            return _currentContext.SaveChanges() > 0;
        }

        /// <summary>
        /// 更新ID集合中所有的实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="int"></typeparam>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int Delete(List<int> ids)
        {
            int completeCount = 0;
            foreach (var id in ids)
            {
                T delModel = GetModel(id);
                this.Entities.Remove(delModel);
                //delModel.IsDeleted = true;
                completeCount += _currentContext.SaveChanges();
            }
            return completeCount;
        }

        public bool Exists(int EntityID)
        {
            return _currentContext.Set<T>().Find(EntityID) != null;
        }

        public bool Exists(Expression<Func<T, bool>> filter)
        {
            return _currentContext.Set<T>().Where(filter).Any();
        }
        public IQueryable<T> GetModel(List<int> ids)
        {
            return _currentContext.Set<T>().Where(t => ids.Contains(t.Id)).AsQueryable<T>();
        }

        public T GetModel(int id)
        {
            return _currentContext.Set<T>().Find(id);
        }

        public dynamic GetModel(Expression<Func<T, dynamic>> selectLambda, Expression<Func<T, bool>> whereLambda)
        {
            var result = _currentContext.Set<T>().Where(whereLambda);
            return result.Select(selectLambda).AsQueryable<dynamic>().FirstOrDefault();
        }

        public T GetModel(Expression<Func<T, T>> selectLambda, Expression<Func<T, bool>> whereLambda)
        {
            var result = _currentContext.Set<T>().Where(whereLambda);
            return result.Select(selectLambda).AsQueryable<T>().FirstOrDefault();
        }

        public int Update(IEnumerable<T> modelList)
        {
            //try
            //{
            var currentContext = (DbContext)_currentContext;
            foreach (T model in modelList)
            {
                currentContext.Entry<T>(model).State = EntityState.Modified;
            }
            return currentContext.SaveChanges();
            //}
            //catch (DbEntityValidationException dbEx)
            //{
            //    throw new Exception(dbEx.Message);
            //}
        }

        public bool Update(T model)
        {
            ((DbContext)_currentContext).Entry<T>(model).State = EntityState.Modified;
            try
            {
                return _currentContext.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public T Add(T input)
        {
            var currentContext = _currentContext;

            currentContext.SaveChanges();
            return input;
        }

    }
}
