using System.Data;

public interface ISalaryRepository
{
    Task<DataTable> GetSalaryOfEmployeeAsync(string employeeId);
}
