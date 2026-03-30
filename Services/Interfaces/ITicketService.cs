public interface ITicketService
{
    Task<DeleteTicketResponse> DeleteTicketAsync(string ticketNumber);
}
