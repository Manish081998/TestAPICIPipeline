public interface IJobOrderService
{
    Task<DeleteOrderResponse> DeleteOrderAsync(string orderNumber);
}
