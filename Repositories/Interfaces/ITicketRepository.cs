public interface ITicketRepository
{
    Task<int> DeleteTicketAsync(string ticketNumber);
    Task<int> InsertTicketDetialsAsync(InsertTicketDetialsRequest request);
}
