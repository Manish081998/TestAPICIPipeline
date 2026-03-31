using Microsoft.Data.SqlClient;
using System.Data;

public static class SqlParamHelper
{
    public static SqlParameter Param(string name, object value, SqlDbType type)
    {
        return new SqlParameter(name, type)
        {
            Value = value ?? DBNull.Value
        };
    }
}
