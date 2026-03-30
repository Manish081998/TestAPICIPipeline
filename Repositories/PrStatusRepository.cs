using System.Data;

public class PrStatusRepository : IPrStatusRepository
{
    private readonly IDbHelper                   _dbHelper;
    private readonly ILogger<PrStatusRepository> _logger;

    public PrStatusRepository(IDbHelper dbHelper, ILogger<PrStatusRepository> logger)
    {
        _dbHelper = dbHelper;
        _logger   = logger;
    }

    public async Task<int> DeletePrStatusAsync(string prNumber)
    {
        try
        {
            var parameters = new[]
            {
                SqlParamHelper.Param("@PRNumber", prNumber, SqlDbType.NVarChar)
            };
            return await _dbHelper.ExecuteNonQueryAsync(
                "sp_deletePrStatus", parameters, CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Repository Error in DeletePrStatusAsync");
            throw;
        }
    }
}
