public interface ISalaryService
{
    Task<SalaryResponse> GetSalaryOfEmployeeAsync(string employeeId);
}
