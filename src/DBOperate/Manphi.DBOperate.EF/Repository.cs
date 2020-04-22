using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Manphi.DbOperate.EF;
using Manphi.DBOperate.Model;

namespace Manphi.DbOperate.EF
{
    public partial class Repository<T> : IRepository<T> where T : BaseEntity
    {

        #region property

        private readonly IBaseDbContext _currentContext;
        private DbSet<T> _entities;

        protected virtual DbSet<T> Entities
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

        #endregion property

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context">上下文对象</param>
        public Repository(IBaseDbContext context) => _currentContext = context;


        //public IDbContextTransaction BeginTransaction()=>_currentContext.BeginTransaction();

        //public void ExecuteStoredProcedureList(string commandText, params object[] parameters) =>_currentContext.ExecuteStoredProcedureList(commandText, parameters);
        #region delete

        #region sync

        /// <summary>
        /// 删除一个ID对应的实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            _currentContext.Set<T>().Remove(GetModel(id));
            return _currentContext.SaveChanges() > 0;
        }

        public bool Delete(T model)
        {
            _currentContext.Set<T>().Remove(GetModel(model.Id));
            return _currentContext.SaveChanges() > 0;
        }

        public bool Delete(IEnumerable<T> models)
        {
            _currentContext.Set<T>().RemoveRange(models);
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
            ids.ForEach(id => _currentContext.Set<T>().Remove(GetModel(id)));
            return _currentContext.SaveChanges();
        }

        #endregion sync

        #region async

        /// <summary>
        /// 删除一个ID对应的实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(int id) => await Task.Run(async () =>
        {
            _currentContext.Set<T>().Remove(await GetModelAsync(id));
            return (await ((DbContext)_currentContext).SaveChangesAsync()) > 0;
        });

        public async Task<int> DeleteAsync(List<int> ids) => await Task.Run(async () =>
        {
            ids.ForEach(async id => _currentContext.Set<T>().Remove(await GetModelAsync(id)));
            return await ((DbContext)_currentContext).SaveChangesAsync();
        });

        #endregion async

        #endregion delete

        #region select

        #region sync

        public bool Exists(int EntityID) => _currentContext.Set<T>().Any(t => EntityID == t.Id);

        public IQueryable<T> GetModel(List<int> ids) => _currentContext.Set<T>().Where(t => ids.Contains(t.Id)).AsQueryable<T>();

        public T GetModel(int id) => _currentContext.Set<T>().Find(id);

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

        public IList<T> GetListAll()
        {
            return _currentContext.Set<T>().OrderByDescending(t => t.Id).ToList();
        }

        #endregion sync

        #region async

        public async Task<bool> ExistsAsync(int EntityID) => await _currentContext.Set<T>().AnyAsync(t => EntityID == t.Id);

        public async Task<IQueryable<T>> GetModelAsync(List<int> ids) => await Task.Run(() => _currentContext.Set<T>().Where(t => ids.Contains(t.Id)).AsQueryable<T>());

        public async Task<T> GetModelAsync(int id) => await _currentContext.Set<T>().FindAsync(id);

        public async Task<dynamic> GetModelAsync(Expression<Func<T, dynamic>> selectLambda, Expression<Func<T, bool>> whereLambda) => await _currentContext.Set<T>().Where(whereLambda).Select(selectLambda).AsQueryable<dynamic>().FirstOrDefaultAsync();

        public async Task<T> GetModelAsync(Expression<Func<T, T>> selectLambda, Expression<Func<T, bool>> whereLambda) => await _currentContext.Set<T>().Where(whereLambda).Select(selectLambda).AsQueryable<T>().FirstOrDefaultAsync();

        public async Task<IList<T>> GetListAllAsync() => await _currentContext.Set<T>().OrderByDescending(t => t.Id).ToListAsync();

        #endregion async

        #endregion select

        #region update

        #region sync

        public int Update(IEnumerable<T> modelList)
        {
            _currentContext.Set<T>().UpdateRange(modelList);
            return _currentContext.SaveChanges();
        }

        public bool Update(T model)
        {
            _currentContext.Set<T>().Update(model);
            return _currentContext.SaveChanges() > 0;
        }

        #endregion sync

        #region updateAsync

        public async Task<int> UpdateAsync(IEnumerable<T> modelList)
        {
            _currentContext.Set<T>().UpdateRange(modelList);
            return await ((DbContext)_currentContext).SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(T model)
        {
            _currentContext.Set<T>().Update(model);
            return (await ((DbContext)_currentContext).SaveChangesAsync()) > 0;
        }

        #endregion updateAsync

        #endregion update

        #region add

        public int Add(IEnumerable<T> input)
        {
            _currentContext.Set<T>().AddRange(input);
            return _currentContext.SaveChanges();
        }

        public T Add(T input)
        {
            _currentContext.Set<T>().Add(input);
            _currentContext.SaveChanges();
            return input;
        }

        public async Task<T> AddAsync(T input)
        {
            await _currentContext.Set<T>().AddAsync(input);
            _currentContext.SaveChanges();
            return input;
        }

        public async Task<int> AddAsync(IEnumerable<T> input)
        {
            await _currentContext.Set<T>().AddRangeAsync(input);
            return await ((DbContext)_currentContext).SaveChangesAsync();
        }
        #endregion add

        public DataTable ExecuteStoredProcedureList(string commandText, params object[] parameters) => _currentContext.ExecuteStoredProcedureList(commandText, parameters);

        public void ExecuteSql(string sql) => _currentContext.SqlQuery(sql, null);
    }
}
