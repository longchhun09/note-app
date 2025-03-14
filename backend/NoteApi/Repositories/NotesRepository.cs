using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using NoteApi.Data.Constants;
using NoteApi.Data.QueryBuilders;
using NoteApi.Models;
using NoteApi.Repositories.Base;
using NoteApi.Repositories.Interfaces;

namespace NoteApi.Repositories
{
    public class NotesRepository : BaseRepository<Note>, INotesRepository
    {
        private readonly NotesQueryBuilder _queryBuilder;

        public NotesRepository(IDapperDbContext dbContext)
            : base(
                dbContext,
                NoteQueries.Create,
                NoteQueries.GetById,
                NoteQueries.Update,
                NoteQueries.Delete
            )
        {
            _queryBuilder = new NotesQueryBuilder();
        }

        public async Task<IEnumerable<Note>> GetAllByUserIdAsync(
            int userId,
            string searchTerm = null,
            string sortField = null,
            string sortOrder = null
        )
        {
            using var connection = _dbContext.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                parameters.Add("@SearchTerm", $"%{searchTerm}%");
            }

            var sql = _queryBuilder.BuildGetAllByUserQuery(searchTerm, sortField, sortOrder);
            return await connection.QueryAsync<Note>(sql, parameters);
        }
    }
};
