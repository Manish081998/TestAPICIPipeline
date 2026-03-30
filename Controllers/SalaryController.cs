using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route(ApiRoutes.Salary.Base)]
public class SalaryController : ControllerBase
{
    private readonly ISalaryService _salaryService;
    private readonly ILogger<SalaryController> _logger;

    public SalaryController(ISalaryService salaryService, ILogger<SalaryController> logger)
    {
        _salaryService = salaryService;
        _logger = logger;
    }

    [HttpGet(ApiRoutes.Salary.Get)]
    public async Task<IActionResult> GetSalaryOfEmployee([FromQuery] GetSalaryRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.EmployeeId))
                return BadRequest(ApiResponse<object>.Fail("EmployeeId is required"));

            var result = await _salaryService.GetSalaryOfEmployeeAsync(request.EmployeeId);
            if (result == null)
                return NotFound(ApiResponse<object>.Fail("Employee salary record not found"));

            return Ok(ApiResponse<SalaryResponse>.Ok(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetSalaryOfEmployee");
            return StatusCode(500, ApiResponse<object>.Fail("Internal Server Error"));
        }
    }

}
