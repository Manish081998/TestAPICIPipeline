public class GetSalaryRequest
{
    public string EmployeeId { get; set; }
}

public class SalaryResponse
{
    public string    EmployeeId   { get; set; }
    public string  EmployeeName { get; set; }
    public string  Department   { get; set; }
    public decimal BasicSalary  { get; set; }
    public decimal Allowance    { get; set; }
    public decimal TotalSalary  { get; set; }
}

