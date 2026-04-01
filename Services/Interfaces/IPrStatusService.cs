public interface IPrStatusService
{
    Task<DeletePrStatusResponse> DeletePrStatusAsync(string prNumber);
}
