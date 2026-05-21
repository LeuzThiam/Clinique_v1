using MaBoutique.Payments.Application.Abstractions;
using MaBoutique.Payments.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaBoutique.Payments.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PaiementController : ControllerBase
    {
        private readonly IPaymentsApplicationService _paymentsService;

        public PaiementController(IPaymentsApplicationService paymentsService)
        {
            _paymentsService = paymentsService;
        }

        [HttpPost("payment-intent")]
        public async Task<IActionResult> CreatePaymentIntent([FromBody] PaiementRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _paymentsService.CreatePaymentIntentAsync(request, cancellationToken);
                return Ok(new
                {
                    clientSecret = result.ClientSecret,
                    amount = result.Amount,
                    currency = result.Currency
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("public-key")]
        public IActionResult GetPublishableKey()
        {
            return Ok(new { key = _paymentsService.GetPublishableKey() });
        }
    }
}
