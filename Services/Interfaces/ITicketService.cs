public interface ITicketService
{
    Task<DeleteTicketResponse>          DeleteTicketAsync(string ticketNumber);
    Task<InsertTicketDetialsResponse>   InsertTicketDetialsAsync(InsertTicketDetialsRequest request);
}
