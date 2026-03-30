using System.Data;

public interface IDepartmentRepository
{
    Task<DataTable> GetDepartmentNameAsync(string departmentId);
}
