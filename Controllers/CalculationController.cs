using System.Text;
using System.Text.Json;
using System.Text.Unicode;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using SpeedBox.Models;
using SpeedBox.Services;

namespace SpeedBox.Controllers;

[ApiController]
[Route("[controller]")]
public class CalculationController : ControllerBase
{
    private readonly ILogger<CalculationController> _logger;


     private readonly ICalculationService _calculationService;

    public CalculationController(ILogger<CalculationController> logger, ICalculationService calculationService)
    {
        _logger = logger;
        _calculationService = calculationService;
    }

    [HttpPost]
    public async Task<IActionResult> Post(CalculationRequest request)
    {
        _logger.LogInformation("Calculation request received");
        var result = await _calculationService.Calculate(request);
        return Ok(result);
    }
   
}
