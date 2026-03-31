using Microsoft.Data.SqlClient;
using System.Data;

public interface IDbHelper
{
    Task<DataTable> ExecuteReaderAsync(string procedureName, SqlParameter[] parameters, CommandType commandType);
    Task<int>       ExecuteNonQueryAsync(string procedureName, SqlParameter[] parameters, CommandType commandType);
}
