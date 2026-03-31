public class CaseService : ICaseService
{
    private readonly ICaseRepository      _caseRepository;
    private readonly ILogger<CaseService> _logger;

    public CaseService(ICaseRepository caseRepository, ILogger<CaseService> logger)
    {
        _caseRepository = caseRepository;
        _logger         = logger;
    }

    public async Task<List<CaseResponse>> GetAllCasesAsync()
    {
        try
        {
            var table = await _caseRepository.GetAllCasesAsync();
            if (table == null || table.Rows.Count == 0)
                return new List<CaseResponse>();

            return table.ToCaseResponseList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Service Error in GetAllCasesAsync");
            throw;
        }
    }
}
