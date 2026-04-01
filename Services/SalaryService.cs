public class SalaryService : ISalaryService
{
    private readonly ISalaryRepository      _salaryRepository;
    private readonly ILogger<SalaryService> _logger;

    public SalaryService(ISalaryRepository salaryRepository, ILogger<SalaryService> logger)
    {
        _salaryRepository = salaryRepository;
        _logger           = logger;
    }

    public async Task<SalaryResponse> GetSalaryOfEmployeeAsync(string employeeId)
    {
        try
        {
            var table = await _salaryRepository.GetSalaryOfEmployeeAsync(employeeId);
            if (table == null || table.Rows.Count == 0)
                return null!;

            return table.ToSalaryResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Service Error in GetSalaryOfEmployeeAsync");
            throw;
        }
    }

}
