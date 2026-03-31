public interface IPrStatusRepository
{
    Task<int> DeletePrStatusAsync(string prNumber);
}
