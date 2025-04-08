using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet("secure")]
    [Authorize] // JWT or API key will both work via CombinedAuthMiddleware
    public IActionResult SecureEndpoint()
    {
        var user = User.Identity?.Name ?? "API Key Client";
        return Ok($"You are authorized as: {user}");
    }
}