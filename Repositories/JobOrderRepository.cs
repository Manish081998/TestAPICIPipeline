using System.Data;

public class JobOrderRepository : IJobOrderRepository
{
    private readonly IDbHelper                     _dbHelper;
    private readonly ILogger<JobOrderRepository>   _logger;

    public JobOrderRepository(IDbHelper dbHelper, ILogger<JobOrderRepository> logger)
    {
        _dbHelper = dbHelper;
        _logger   = logger;
    }

    public async Task<int> DeleteOrderAsync(string orderNumber)
    {
        try
        {
            var parameters = new[]
            {
                SqlParamHelper.Param("@OrderNumber", orderNumber, SqlDbType.NVarChar)
            };
            return await _dbHelper.ExecuteNonQueryAsync(
                "sp_deleteOrderAsPerOrderNumber", parameters, CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Repository Error in DeleteOrderAsync");
            throw;
        }
    }
}
