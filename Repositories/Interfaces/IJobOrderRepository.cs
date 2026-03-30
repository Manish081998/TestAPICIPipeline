public interface IJobOrderRepository
{
    Task<int> DeleteOrderAsync(string orderNumber);
}
