using Microsoft.Data.SqlClient;
using System.Data;

public class DbHelper : IDbHelper
{
    private readonly string                _connectionString;
    private readonly ILogger<DbHelper>     _logger;

    public DbHelper(IConfiguration configuration, ILogger<DbHelper> logger)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        _logger = logger;
    }

    public async Task<DataTable> ExecuteReaderAsync(string procedureName, SqlParameter[] parameters, CommandType commandType)
    {
        var table = new DataTable();
        await using var connection = new SqlConnection(_connectionString);
        await using var command    = new SqlCommand(procedureName, connection)
        {
            CommandType = commandType
        };

        if (parameters != null)
            command.Parameters.AddRange(parameters);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();
        table.Load(reader);
        return table;
    }

    public async Task<int> ExecuteNonQueryAsync(string procedureName, SqlParameter[] parameters, CommandType commandType)
    {
        await using var connection = new SqlConnection(_connectionString);
        await using var command    = new SqlCommand(procedureName, connection)
        {
            CommandType = commandType
        };

        if (parameters != null)
            command.Parameters.AddRange(parameters);

        await connection.OpenAsync();
        return await command.ExecuteNonQueryAsync();
    }
}
