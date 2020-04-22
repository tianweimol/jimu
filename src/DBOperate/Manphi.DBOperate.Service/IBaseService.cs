using Mol.DBOperate.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Manphi.DBOperate.Service
{
    public interface IBaseService
    {
    }
    public interface IBaseService<T> where T : BaseEntity
    {
        #region select

        #region sync

        IQueryable<dynamic> GetListByPage<TKey>(Expression<Func<T, dynamic>> selectLambda, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda, int pageSize, int pageIndex, out int total, bool isAsc);

        IQueryable<T> GetListByPage<TKey>(Expression<Func<T, T>> selectLambda, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda, int pageSize, int pageIndex, out int total, bool isAsc);

        IQueryable<dynamic> GetListByPageBase<TKey>(Expression<Func<T, dynamic>> selectLambda, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda, bool isAsc = true);

        IQueryable<T> GetListByPageBase<TKey>(Expression<Func<T, T>> selectLambda, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda, bool isAsc = true);

        IQueryable<T> GetListByPageBase<TKey>(Expression<Func<T, T>> selectLambda, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda, Expression<Func<T, TKey>> thenByLambda, Expression<Func<T, TKey>> thenByDescLambda, bool isAsc = true, bool thenByFirs = true);

        IList<T> GetListAll();

        /// <summary>
        /// 根据ID查询实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetModel(int id);

        /// <summary>
        /// 根据ID集合查询实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids"></param>
        /// <returns></returns>
        IQueryable<T> GetModel(List<int> ids);

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        bool Exists(int EntityID);

        //IQueryable<T> GetModel(List<int> ids);
        T GetModel(Expression<Func<T, T>> selectLambda, Expression<Func<T, bool>> whereLambda);

        dynamic GetModel(Expression<Func<T, dynamic>> selectLambda, Expression<Func<T, bool>> whereLambda);

        #endregion sync

        #region async

        Task<IList<T>> GetListAllAsync();

        /// <summary>
        /// 根据ID查询实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> GetModelAsync(int id);

        /// <summary>
        /// 根据ID集合查询实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IQueryable<T>> GetModelAsync(List<int> ids);

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        Task<bool> ExistsAsync(int EntityID);

        //IQueryable<T> GetModel(List<int> ids);
        Task<T> GetModelAsync(Expression<Func<T, T>> selectLambda, Expression<Func<T, bool>> whereLambda);

        Task<dynamic> GetModelAsync(Expression<Func<T, dynamic>> selectLambda, Expression<Func<T, bool>> whereLambda);

        #endregion async

        #endregion select

        #region update

        #region sync

        /// <summary>
        /// 更新单个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="int"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        bool Update(T model);

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="modelList"></param>
        /// <returns></returns>
        int Update(IEnumerable<T> modelList);

        #endregion sync

        #region async

        /// <summary>
        /// 更新单个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="int"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(T model);

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="modelList"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(IEnumerable<T> modelList);

        #endregion async

        #endregion update

        #region Delete

        #region sync

        bool Delete(int id);

        int Delete(List<int> ids);

        #endregion sync

        #region async

        Task<bool> DeleteAsync(int id);

        Task<int> DeleteAsync(List<int> ids);

        #endregion async

        #endregion Delete

        #region Add

        #region sync

        /// <summary>
        /// save a entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        T Add(T input);

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        int Add(IEnumerable<T> input);

        #endregion sync

        #region async

        Task<T> AddAsync(T input);

        Task<int> AddAsync(IEnumerable<T> input);

        #endregion async

        #endregion Add

        DataTable ExecuteStoredProcedureList(string commandText, params object[] parameters);
        void ExecuteSql(string sql);
    }
}
