using Manphi.DBOperate.EF;
using Mol.DBOperate.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Manphi.DBOperate.Service
{
    public class BaseService<T> : IBaseService<T> where T : BaseEntity
    {
        #region property
        public readonly IRepository<T> _commonRepository;
        #endregion property

        public BaseService(IRepository<T> commonRepository  )
        {
            this._commonRepository = commonRepository;
        }
        #region function

        #region select

        #region sync

        public IQueryable<T> GetListByPage<TKey>(Expression<Func<T, T>> selectLambda, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda, int pageSize, int pageIndex, out int total, bool isAsc)
        {
            return _commonRepository.GetListByPage(selectLambda, whereLambda, orderLambda, pageSize, pageIndex, out total, isAsc);
        }

        public IQueryable<dynamic> GetListByPage<TKey>(Expression<Func<T, dynamic>> selectLambda, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda, int pageSize, int pageIndex, out int total, bool isAsc)
        {
            return _commonRepository.GetListByPage(selectLambda, whereLambda, orderLambda, pageSize, pageIndex, out total, isAsc);
        }

        public IQueryable<T> GetListByPageBase<TKey>(Expression<Func<T, T>> selectLambda, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda, bool isAsc)
        {
            return _commonRepository.GetListByPageBase(selectLambda, whereLambda, orderLambda, isAsc);
        }

        public IQueryable<T> GetListByPageBase<TKey>(Expression<Func<T, T>> selectLambda, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda, Expression<Func<T, TKey>> thenByLambda, Expression<Func<T, TKey>> thenByDescLambda, bool isAsc = true, bool thenByFirs = true)
        {
            return _commonRepository.GetListByPageBase(selectLambda, whereLambda, orderLambda, thenByLambda, thenByDescLambda, isAsc, thenByFirs);
        }

        public IQueryable<dynamic> GetListByPageBase<TKey>(Expression<Func<T, dynamic>> selectLambda, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda, bool isAsc)
        {
            return _commonRepository.GetListByPageBase(selectLambda, whereLambda, orderLambda, isAsc);
        }

        public bool Exists(int EntityID)
        {
            return _commonRepository.Exists(EntityID);
        }

        public IList<T> GetListAll()
        {
            return _commonRepository.GetListAll();
        }

        public IQueryable<T> GetModel(List<int> ids)
        {
            return _commonRepository.GetModel(ids);
        }

        public T GetModel(int id)
        {
            return _commonRepository.GetModel(id);
        }

        public dynamic GetModel(Expression<Func<T, dynamic>> selectLambda, Expression<Func<T, bool>> whereLambda)
        {
            return _commonRepository.GetModel(selectLambda, whereLambda);
        }

        public T GetModel(Expression<Func<T, T>> selectLambda, Expression<Func<T, bool>> whereLambda)
        {
            return _commonRepository.GetModel(selectLambda, whereLambda);
        }

        #endregion sync

        #region async

        public async Task<bool> ExistsAsync(int EntityID) => await _commonRepository.ExistsAsync(EntityID);

        public async Task<IList<T>> GetListAllAsync() => await _commonRepository.GetListAllAsync();

        public async Task<IQueryable<T>> GetModelAsync(List<int> ids) => await _commonRepository.GetModelAsync(ids);

        public async Task<T> GetModelAsync(int id) => await _commonRepository.GetModelAsync(id);

        public async Task<dynamic> GetModelAsync(Expression<Func<T, dynamic>> selectLambda, Expression<Func<T, bool>> whereLambda) => await _commonRepository.GetModelAsync(selectLambda, whereLambda);

        public async Task<T> GetModelAsync(Expression<Func<T, T>> selectLambda, Expression<Func<T, bool>> whereLambda) => await _commonRepository.GetModelAsync(selectLambda, whereLambda);

        #endregion async

        #endregion select

        #region update

        #region sync

        public int Update(IEnumerable<T> modelList)
        {
            var re = _commonRepository.Update(modelList);
            return re;
        }

        public bool Update(T model)
        {
            var re = _commonRepository.Update(model);
            return re;
        }

        #endregion sync

        #region async

        public async Task<int> UpdateAsync(IEnumerable<T> modelList) => await _commonRepository.UpdateAsync(modelList);

        public async Task<bool> UpdateAsync(T model) => await _commonRepository.UpdateAsync(model);

        #endregion async

        #endregion update

        #region Delete

        #region sync

        public int Delete(List<int> ids)
        {
            var model = GetModel(ids.First());
            var re = _commonRepository.Delete(ids);
            return re;
        }

        public bool Delete(int id)
        {
            var model = GetModel(id);
            var re = _commonRepository.Delete(id);
            return re;
        }

        #endregion sync

        #region async

        public async Task<int> DeleteAsync(List<int> ids) => await _commonRepository.DeleteAsync(ids);

        public async Task<bool> DeleteAsync(int id) => await _commonRepository.DeleteAsync(id);

        #endregion async

        #endregion Delete

        #region Add

        #region sync

        public int Add(IEnumerable<T> input)
        {
            var re = _commonRepository.Add(input);
            return re;
        }

        public T Add(T input)
        {
            var re = _commonRepository.Add(input);
            return re;
        }

        #endregion sync

        #region async

        public async Task<int> AddAsync(IEnumerable<T> input) => await _commonRepository.AddAsync(input);

        public async Task<T> AddAsync(T input) => await _commonRepository.AddAsync(input);

        #endregion async

        #endregion Add


        public DataTable ExecuteStoredProcedureList(string commandText, params object[] parameters) => _commonRepository.ExecuteStoredProcedureList(commandText, parameters);
        public void ExecuteSql(string sql) => _commonRepository.ExecuteSql(sql);
        #endregion function
    }
}
