using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route(ApiRoutes.JobOrder.Base)]
public class JobOrderController : ControllerBase
{
    private readonly IJobOrderService            _jobOrderService;
    private readonly ILogger<JobOrderController> _logger;

    public JobOrderController(IJobOrderService jobOrderService, ILogger<JobOrderController> logger)
    {
        _jobOrderService = jobOrderService;
        _logger          = logger;
    }

    [HttpDelete(ApiRoutes.JobOrder.DeleteOrder)]
    public async Task<IActionResult> DeleteOrder([FromQuery] DeleteOrderRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.OrderNumber))
                return BadRequest(ApiResponse<object>.Fail("OrderNumber is required"));

            var result = await _jobOrderService.DeleteOrderAsync(request.OrderNumber);
            if (result == null)
                return NotFound(ApiResponse<object>.Fail("Order not found"));

            return Ok(ApiResponse<DeleteOrderResponse>.Ok(result, "Order deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in DeleteOrder");
            return StatusCode(500, ApiResponse<object>.Fail("Internal Server Error"));
        }
    }
}
