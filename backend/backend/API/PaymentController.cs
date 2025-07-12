using backend.Application;
using backend.Domain;
using Microsoft.AspNetCore.Mvc;

namespace backend.API
{
  [ApiController]
  [Route("api/[controller]")]
  public class PaymentController : ControllerBase
  {
    private readonly IPaymentOrchestrator paymentOrchestrator;

    public PaymentController(IPaymentOrchestrator paymentOrchestrator)
    {
      this.paymentOrchestrator = paymentOrchestrator;
    }

    [HttpPost]
    public IActionResult PayProducts(
      [FromBody] PaymentRequestModel paymentRequest)
    {
      try
      {
        var result
          = paymentOrchestrator.ProcessPayment(
              paymentRequest.CoinsPayment, paymentRequest.Products);

        return Ok(result);
      }
      catch (Exception)
      {
        return StatusCode(500, "Could not process payment at this time.");
      }
    }
  }
}
