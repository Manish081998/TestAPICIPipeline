using System.Data;
using Microsoft.Data.SqlClient;

public class CaseRepository : ICaseRepository
{
    private readonly IDbHelper               _dbHelper;
    private readonly ILogger<CaseRepository> _logger;

    public CaseRepository(IDbHelper dbHelper, ILogger<CaseRepository> logger)
    {
        _dbHelper = dbHelper;
        _logger   = logger;
    }

    public async Task<DataTable> GetAllCasesAsync()
    {
        try
        {
            return await _dbHelper.ExecuteReaderAsync(
                "sp_GetallCases", Array.Empty<SqlParameter>(), CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Repository Error in GetAllCasesAsync");
            throw;
        }
    }
}
