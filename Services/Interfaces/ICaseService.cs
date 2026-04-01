public interface ICaseService
{
    Task<List<CaseResponse>> GetAllCasesAsync();
}
