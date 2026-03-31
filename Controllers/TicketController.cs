using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route(ApiRoutes.Ticket.Base)]
public class TicketController : ControllerBase
{
    private readonly ITicketService            _ticketService;
    private readonly ILogger<TicketController> _logger;

    public TicketController(ITicketService ticketService, ILogger<TicketController> logger)
    {
        _ticketService = ticketService;
        _logger        = logger;
    }
    [HttpDelete(ApiRoutes.Ticket.Delete)]
    public async Task<IActionResult> DeleteTicket([FromQuery] DeleteTicketRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.TicketNumber))
                return BadRequest(ApiResponse<object>.Fail("TicketNumber is required"));

            var result = await _ticketService.DeleteTicketAsync(request.TicketNumber);
            if (result == null)
                return NotFound(ApiResponse<object>.Fail("Ticket not found"));

            return Ok(ApiResponse<DeleteTicketResponse>.Ok(result, "Ticket deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in DeleteTicket");
            return StatusCode(500, ApiResponse<object>.Fail("Internal Server Error"));
        }
    }

//31-03-2026
    [HttpPost(ApiRoutes.Ticket.InsertTicketDetials)]
    public async Task<IActionResult> InsertTicketDetials([FromBody] InsertTicketDetialsRequest request)
    {
        try
        {
            var result = await _ticketService.InsertTicketDetialsAsync(request);
            return Ok(ApiResponse<InsertTicketDetialsResponse>.Ok(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in InsertTicketDetials");
            return StatusCode(500, ApiResponse<object>.Fail("Internal Server Error"));
        }
    }
}
