using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using NoteApi.Models;

namespace NoteApi.Repositories.Base
{
    public interface IBaseRepository<T>
        where T : class
    {
        Task<int> CreateAsync(T entity);
        Task<T> GetByIdAsync(int id, int userId);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id, int userId);
    }

    public abstract class BaseRepository<T> : IBaseRepository<T>
        where T : class
    {
        protected readonly IDapperDbContext _dbContext;
        protected readonly string _createQuery;
        protected readonly string _getByIdQuery;
        protected readonly string _updateQuery;
        protected readonly string _deleteQuery;

        public BaseRepository(
            IDapperDbContext dbContext,
            string createQuery,
            string getByIdQuery,
            string updateQuery,
            string deleteQuery
        )
        {
            _dbContext = dbContext;
            _createQuery = createQuery;
            _getByIdQuery = getByIdQuery;
            _updateQuery = updateQuery;
            _deleteQuery = deleteQuery;
        }

        public virtual async Task<int> CreateAsync(T entity)
        {
            using var connection = _dbContext.CreateConnection();
            return await connection.QuerySingleAsync<int>(_createQuery, entity);
        }

        public virtual async Task<bool> DeleteAsync(int id, int userId)
        {
            using var connection = _dbContext.CreateConnection();
            var updatedRow = await connection.ExecuteAsync(
                _deleteQuery,
                new { Id = id, UserId = userId }
            );
            return updatedRow > 0;
        }

        public virtual async Task<T> GetByIdAsync(int id, int userId)
        {
            using var connection = _dbContext.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<T>(
                _getByIdQuery,
                new { Id = id, UserId = userId }
            );
        }

        public virtual async Task<bool> UpdateAsync(T entity)
        {
            using var connection = _dbContext.CreateConnection();
            var updatedRow = await connection.ExecuteAsync(_updateQuery, entity);
            return updatedRow > 0;
        }
    }
}
