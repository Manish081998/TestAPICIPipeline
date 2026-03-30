using System.Data;

public static class CaseTranslator
{
    public static List<CaseResponse> ToCaseResponseList(this DataTable table)
    {
        if (table == null || table.Rows.Count == 0)
            return new List<CaseResponse>();

        return table.AsEnumerable().Select(row => new CaseResponse
        {
            ID     = row.Field<long>("ID"),
            Status = row.Field<string>("Status"),
            CnNo   = row.Field<string>("CN#"),
            SoNo   = row.Field<string>("SO#")
        }).ToList();
    }
}
