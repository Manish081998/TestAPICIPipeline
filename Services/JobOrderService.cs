public class JobOrderService : IJobOrderService
{
    private readonly IJobOrderRepository      _jobOrderRepository;
    private readonly ILogger<JobOrderService> _logger;

    public JobOrderService(IJobOrderRepository jobOrderRepository, ILogger<JobOrderService> logger)
    {
        _jobOrderRepository = jobOrderRepository;
        _logger             = logger;
    }

    public async Task<DeleteOrderResponse> DeleteOrderAsync(string orderNumber)
    {
        try
        {
            var rowsAffected = await _jobOrderRepository.DeleteOrderAsync(orderNumber);
            if (rowsAffected == 0)
                return null!;

            return new DeleteOrderResponse { OrderNumber = orderNumber };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Service Error in DeleteOrderAsync");
            throw;
        }
    }
}
