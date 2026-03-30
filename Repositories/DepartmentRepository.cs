using System.Data;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly IDbHelper                     _dbHelper;
    private readonly ILogger<DepartmentRepository> _logger;

    public DepartmentRepository(IDbHelper dbHelper, ILogger<DepartmentRepository> logger)
    {
        _dbHelper = dbHelper;
        _logger   = logger;
    }

    public async Task<DataTable> GetDepartmentNameAsync(string departmentId)
    {
        try
        {
            var parameters = new[]
            {
                SqlParamHelper.Param("@DepartmentId", departmentId, SqlDbType.NVarChar)
            };
            return await _dbHelper.ExecuteReaderAsync(
                "sp_GetDepartmentName", parameters, CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Repository Error in GetDepartmentNameAsync");
            throw;
        }
    }
}
