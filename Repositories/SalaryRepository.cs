using System.Data;

public class SalaryRepository : ISalaryRepository
{
    private readonly IDbHelper                  _dbHelper;
    private readonly ILogger<SalaryRepository>  _logger;

    public SalaryRepository(IDbHelper dbHelper, ILogger<SalaryRepository> logger)
    {
        _dbHelper = dbHelper;
        _logger   = logger;
    }

    public async Task<DataTable> GetSalaryOfEmployeeAsync(string employeeId)
    {
        try
        {
            var parameters = new[]
            {
                SqlParamHelper.Param("@EmployeeId", employeeId, SqlDbType.UniqueIdentifier)
            };
            return await _dbHelper.ExecuteReaderAsync(
                "sp_GetSalaryOfEmployee", parameters, CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Repository Error in GetSalaryOfEmployeeAsync");
            throw;
        }
    }

}
