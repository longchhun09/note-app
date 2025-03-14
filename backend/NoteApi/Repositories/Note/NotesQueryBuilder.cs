using NoteApi.Data.Constants;

namespace NoteApi.Data.QueryBuilders
{
    public class NotesQueryBuilder
    {
        public string BuildGetAllByUserQuery(string searchTerm, string sortField, string sortOrder)
        {
            var sql = NoteQueries.GetAllBase;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                sql += @" AND (Title LIKE @SearchTerm OR Content LIKE @SearchTerm)";
            }

            sql += BuildOrderByClause(sortField, sortOrder);

            return sql;
        }

        private string BuildOrderByClause(string sortField, string sortOrder)
        {
            var orderBy = " ORDER BY ";

            if (string.IsNullOrWhiteSpace(sortField))
            {
                return orderBy + "UpdatedAt DESC";
            }

            string field = sortField.ToLower() switch
            {
                "title" => "Title",
                "createdat" => "CreatedAt",
                "updatedat" => "UpdatedAt",
                _ => "UpdatedAt",
            };

            string direction = sortOrder?.ToLower() == "asc" ? "ASC" : "DESC";

            return orderBy + field + " " + direction;
        }
    }
}
