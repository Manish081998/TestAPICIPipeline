using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route(ApiRoutes.Case.Base)]
public class CaseController : ControllerBase
{
    private readonly ICaseService            _caseService;
    private readonly ILogger<CaseController> _logger;

    public CaseController(ICaseService caseService, ILogger<CaseController> logger)
    {
        _caseService = caseService;
        _logger      = logger;
    }

    [HttpGet(ApiRoutes.Case.GetCase)]
    public async Task<IActionResult> GetAllCases()
    {
        try
        {
            var result = await _caseService.GetAllCasesAsync();
            return Ok(ApiResponse<List<CaseResponse>>.Ok(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllCases");
            return StatusCode(500, ApiResponse<object>.Fail("Internal Server Error"));
        }
    }
}
