public interface ITicketRepository
{
    Task<int> DeleteTicketAsync(string ticketNumber);
}
