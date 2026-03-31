using System.Data;

public interface ICaseRepository
{
    Task<DataTable> GetAllCasesAsync();
}
