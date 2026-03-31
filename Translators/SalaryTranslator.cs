using System.Data;

public static class SalaryTranslator
{
    public static SalaryResponse ToSalaryResponse(this DataTable table)
    {
        if (table == null || table.Rows.Count == 0)
            return null;

        var row = table.Rows[0];
        return new SalaryResponse
        {
            EmployeeId   = row.Field<string>("EmployeeId"),
            EmployeeName = row.Field<string>("EmployeeName"),
            Department   = row.Field<string>("Department"),
            BasicSalary  = row.Field<decimal>("BasicSalary"),
            Allowance    = row.Field<decimal>("Allowance"),
            TotalSalary  = row.Field<decimal>("TotalSalary")
        };
    }

}
