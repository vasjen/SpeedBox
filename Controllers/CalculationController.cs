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
    [ProducesResponseType(typeof(CalculationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post(CalculationRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        _logger.LogInformation("Calculation request received");
        var result = await _calculationService.Calculate(request);
        if (result == null)
        {
            return BadRequest("City not found or other problem. Check your request");
        }
        return Ok(result);
    }
   
}
