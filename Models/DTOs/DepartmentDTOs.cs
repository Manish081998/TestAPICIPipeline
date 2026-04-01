public class GetDepartmentNameRequest
{
    public string DepartmentId { get; set; }
}

public class DepartmentNameResponse
{
    public string  DepartmentId   { get; set; }
    public string  DepartmentName { get; set; }
    public string  Department     { get; set; }
    public decimal Allowance      { get; set; }
    public decimal TotalSalary    { get; set; }
}
