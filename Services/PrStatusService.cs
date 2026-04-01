public class PrStatusService : IPrStatusService
{
    private readonly IPrStatusRepository      _prStatusRepository;
    private readonly ILogger<PrStatusService> _logger;

    public PrStatusService(IPrStatusRepository prStatusRepository, ILogger<PrStatusService> logger)
    {
        _prStatusRepository = prStatusRepository;
        _logger             = logger;
    }

    public async Task<DeletePrStatusResponse> DeletePrStatusAsync(string prNumber)
    {
        try
        {
            var rowsAffected = await _prStatusRepository.DeletePrStatusAsync(prNumber);
            if (rowsAffected == 0)
                return null!;

            return PrStatusTranslator.ToDeletePrStatusResponse(prNumber);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Service Error in DeletePrStatusAsync");
            throw;
        }
    }
}
