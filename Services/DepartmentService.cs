public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository      _departmentRepository;
    private readonly ILogger<DepartmentService> _logger;

    public DepartmentService(IDepartmentRepository departmentRepository, ILogger<DepartmentService> logger)
    {
        _departmentRepository = departmentRepository;
        _logger               = logger;
    }

    public async Task<DepartmentNameResponse> GetDepartmentNameAsync(string departmentId)
    {
        try
        {
            var table = await _departmentRepository.GetDepartmentNameAsync(departmentId);
            if (table == null || table.Rows.Count == 0)
                return null!;

            return table.ToDepartmentNameResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Service Error in GetDepartmentNameAsync");
            throw;
        }
    }
}
