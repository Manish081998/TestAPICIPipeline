using System.Data;

public static class DepartmentTranslator
{
    public static DepartmentNameResponse ToDepartmentNameResponse(this DataTable table)
    {
        if (table == null || table.Rows.Count == 0)
            return null;

        var row = table.Rows[0];
        return new DepartmentNameResponse
        {
            DepartmentId   = row.Field<string>("DepartmentId"),
            DepartmentName = row.Field<string>("DepartmentName"),
            Department     = row.Field<string>("Department"),
            Allowance      = row.Field<decimal>("Allowance"),
            TotalSalary    = row.Field<decimal>("TotalSalary")
        };
    }
}
