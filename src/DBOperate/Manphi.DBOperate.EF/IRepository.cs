using Manphi.DBOperate.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Manphi.DbOperate.EF
{
    public interface IRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// 获得IQueryable对象
        /// </summary>
        IQueryable<T> Table { get; }

        /// <summary>
        /// No tracking 实体不被EF context跟踪，仅用于只读操作
        /// </summary>
        IQueryable<T> TableNoTracking { get; }

        //IDbContextTransaction BeginTransaction();
        //void ExecuteStoredProcedureList(string commandText, params object[] parameters);

        #region delete

        #region sync

        /// <summary>
        /// 删除一个ID对应的实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Delete(int id);

        int Delete(List<int> ids);

        bool Delete(T model);

        bool Delete(IEnumerable<T> models);

        #endregion sync

        #region async

        /// <summary>
        /// 删除一个ID对应的实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(int id);

        Task<int> DeleteAsync(List<int> ids);

        #endregion async

        #endregion delete

        #region add

        T Add(T input);

        /// <summary>
        /// 增加多个实体
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        int Add(IEnumerable<T> input);

        Task<T> AddAsync(T input);

        Task<int> AddAsync(IEnumerable<T> input);

        #endregion add

        #region select

        #region sync

        /// <summary>
        /// 查询动态对象集合，不分页
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="selectLambda"></param>
        /// <param name="whereLambda"></param>
        /// <param name="orderLambda"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        IQueryable<dynamic> GetListByPageBase<TKey>(Expression<Func<T, dynamic>> selectLambda, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda, bool isAsc = true);

        /// <summary>
        /// 查询特定对象集合，不分页
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="selectLambda"></param>
        /// <param name="whereLambda"></param>
        /// <param name="orderLambda"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        IQueryable<T> GetListByPageBase<TKey>(Expression<Func<T, T>> selectLambda, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda, bool isAsc = true);

        IQueryable<T> GetListByPageBase<TKey>(Expression<Func<T, T>> selectLambda, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda, Expression<Func<T, TKey>> thenByLambda, Expression<Func<T, TKey>> thenByDescLambda, bool isAsc = true, bool thenByFirs = true);

        /// <summary>
        /// 查询一个动态对象集合（分页）
        /// </summary>
        /// <typeparam name="TKey">排序类型</typeparam>
        /// <param name="selectLambda">查询对象表达式</param>
        /// <param name="whereLambda">过滤条件表达式</param>
        /// <param name="orderLambda">排序条件表达式</param>
        /// <param name="pageSize">页面上显示几条</param>
        /// <param name="pageIndex">当前第几页</param>
        /// <param name="total">总条数</param>
        /// <param name="isAsc">是否升序排序</param>
        /// <returns></returns>
        IQueryable<dynamic> GetListByPage<TKey>(Expression<Func<T, dynamic>> selectLambda, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda, int pageSize, int pageIndex, out int total, bool isAsc = true);

        /// <summary>
        /// 查询一个特定类型对象的集合（分页）
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="selectLambda"></param>
        /// <param name="whereLambda"></param>
        /// <param name="orderLambda"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="total"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        IQueryable<T> GetListByPage<TKey>(Expression<Func<T, T>> selectLambda, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda, int pageSize, int pageIndex, out int total, bool isAsc = true);

        /// <summary>
        /// 查询所有的实体集合
        /// </summary>
        /// <returns></returns>
        IList<T> GetListAll();

        T GetModel(int id);

        dynamic GetModel(Expression<Func<T, dynamic>> selectLambda, Expression<Func<T, bool>> whereLambda);

        T GetModel(Expression<Func<T, T>> selectLambda, Expression<Func<T, bool>> whereLambda);

        bool Exists(int EntityID);

        IQueryable<T> GetModel(List<int> ids);

        #endregion sync

        #region async

        Task<T> GetModelAsync(int id);

        Task<dynamic> GetModelAsync(Expression<Func<T, dynamic>> selectLambda, Expression<Func<T, bool>> whereLambda);

        Task<T> GetModelAsync(Expression<Func<T, T>> selectLambda, Expression<Func<T, bool>> whereLambda);

        Task<bool> ExistsAsync(int EntityID);

        Task<IQueryable<T>> GetModelAsync(List<int> ids);

        Task<IList<T>> GetListAllAsync();

        #endregion async

        #endregion select

        #region update

        #region sync

        int Update(IEnumerable<T> modelList);

        bool Update(T model);

        #endregion sync

        #region async

        Task<int> UpdateAsync(IEnumerable<T> modelList);

        Task<bool> UpdateAsync(T model);

        #endregion async

        #endregion update
        DataTable ExecuteStoredProcedureList(string commandText, params object[] parameters);
        void ExecuteSql(string sql);
    }
}
