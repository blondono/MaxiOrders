﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;

namespace MaxiOrders.Back.Common
{
    public interface IRepository<T> where T : class
    {
        T Add(T entity);
        void AddRange(IEnumerable<T> entityList);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entityList);
        void Delete(T entity);
        void Delete(long id);
        void Delete(Expression<Func<T, bool>> where);
        T GetById(params object[] id);
        T Get(Expression<Func<T, bool>> where);
        IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");
        IEnumerable<T> GetQueryable(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAllQueryable();
        int Commit();
        void ExecuteSqlCommand(string procedureName, object parameteres);
        DataSet ExecuteStoreProcedure(string sqlQuery, List<DbParameter> parameters);
        IEnumerable<TEntityVO> ExecuteStoreProcedure<TEntityVO>(string procedureName, object parameters);


        //Task<T> FindAllAsync(System.Linq.Expressions.Expression<Func<T, bool>> where);
        //Task<T> SaveAsync();
    }
}
