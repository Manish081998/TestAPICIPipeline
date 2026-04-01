public class TicketService : ITicketService
{
    private readonly ITicketRepository      _ticketRepository;
    private readonly ILogger<TicketService> _logger;

    public TicketService(ITicketRepository ticketRepository, ILogger<TicketService> logger)
    {
        _ticketRepository = ticketRepository;
        _logger           = logger;
    }

    public async Task<DeleteTicketResponse> DeleteTicketAsync(string ticketNumber)
    {
        try
        {
            var rowsAffected = await _ticketRepository.DeleteTicketAsync(ticketNumber);
            if (rowsAffected == 0)
                return null!;

            return new DeleteTicketResponse { TicketNumber = ticketNumber };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Service Error in DeleteTicketAsync");
            throw;
        }
    }

    public async Task<InsertTicketDetialsResponse> InsertTicketDetialsAsync(InsertTicketDetialsRequest request)
    {
        try
        {
            var rowsAffected = await _ticketRepository.InsertTicketDetialsAsync(request);
            return new InsertTicketDetialsResponse
            {
                CaseNumber = request.CaseNumber,
                SoNumber   = request.SoNumber,
                Success    = rowsAffected > 0
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Service Error in InsertTicketDetialsAsync");
            throw;
        }
    }
}
