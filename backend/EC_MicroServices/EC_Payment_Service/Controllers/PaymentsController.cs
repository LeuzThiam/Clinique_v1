using EC_Payment_Service.Models;
using EC_Payment_Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace EC_Payment_Service.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
    private readonly IStripePaymentService _stripePaymentService;

    public PaymentsController(IStripePaymentService stripePaymentService)
    {
        _stripePaymentService = stripePaymentService;
    }

    [HttpGet("public-key")]
    public IActionResult GetPublishableKey()
    {
        return Ok(new
        {
            key = _stripePaymentService.GetPublishableKey(),
            configured = _stripePaymentService.IsConfigured()
        });
    }

    [HttpPost("payment-intent")]
    public async Task<IActionResult> CreatePaymentIntent([FromBody] CreatePaymentIntentRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _stripePaymentService.CreatePaymentIntentAsync(request.Amount, cancellationToken);
            return Ok(new
            {
                clientSecret = result.ClientSecret,
                amount = result.Amount,
                currency = result.Currency
            });
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { error = exception.Message });
        }
        catch (InvalidOperationException exception)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new { error = exception.Message });
        }
    }
}
