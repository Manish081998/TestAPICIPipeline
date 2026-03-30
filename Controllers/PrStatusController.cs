using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route(ApiRoutes.PrStatus.Base)]
public class PrStatusController : ControllerBase
{
    private readonly IPrStatusService            _prStatusService;
    private readonly ILogger<PrStatusController> _logger;

    public PrStatusController(IPrStatusService prStatusService, ILogger<PrStatusController> logger)
    {
        _prStatusService = prStatusService;
        _logger          = logger;
    }

    [HttpDelete(ApiRoutes.PrStatus.DeletePrStatus)]
    public async Task<IActionResult> DeletePrStatus([FromQuery] DeletePrStatusRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.PRNumber))
                return BadRequest(ApiResponse<object>.Fail("PRNumber is required"));

            var result = await _prStatusService.DeletePrStatusAsync(request.PRNumber);
            if (result == null)
                return NotFound(ApiResponse<object>.Fail("PR not found"));

            return Ok(ApiResponse<DeletePrStatusResponse>.Ok(result, "PR deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in DeletePrStatus");
            return StatusCode(500, ApiResponse<object>.Fail("Internal Server Error"));
        }
    }
}
