using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route(ApiRoutes.Salary.Base)]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentService _departmentService;
    private readonly ILogger<DepartmentController> _logger;

    public DepartmentController(IDepartmentService departmentService, ILogger<DepartmentController> logger)
    {
        _departmentService = departmentService;
        _logger            = logger;
    }

    [HttpGet(ApiRoutes.Salary.GetDepartmentName)]
    public async Task<IActionResult> GetDepartmentName([FromQuery] GetDepartmentNameRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.DepartmentId))
                return BadRequest(ApiResponse<object>.Fail("DepartmentId is required"));

            var result = await _departmentService.GetDepartmentNameAsync(request.DepartmentId);
            if (result == null)
                return NotFound(ApiResponse<object>.Fail("Department record not found"));

            return Ok(ApiResponse<DepartmentNameResponse>.Ok(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetDepartmentName");
            return StatusCode(500, ApiResponse<object>.Fail("Internal Server Error"));
        }
    }
}
