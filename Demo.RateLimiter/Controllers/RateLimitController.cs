using Demo.RateLimiter.Services;
using Microsoft.AspNetCore.Mvc;

namespace Demo.RateLimiter.Controllers;

[ApiController]
[Route("[controller]")]
public class RateLimitController : Controller
{
    private readonly ILogger<RateLimitController> _logger;

    public RateLimitController(ILogger<RateLimitController> logger)
    {
        _logger = logger;
    }
    public IActionResult Index()
    {
        var leakyBucket = new LeakyBucket(5,1);

    }
}

