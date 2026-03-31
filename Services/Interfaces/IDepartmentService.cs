public interface IDepartmentService
{
    Task<DepartmentNameResponse> GetDepartmentNameAsync(string departmentId);
}
