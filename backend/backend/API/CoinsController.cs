using backend.Application;
using Microsoft.AspNetCore.Mvc;

namespace backend.API
{
  [ApiController]
  [Route("api/[controller]")]
  public class CoinsController : ControllerBase
  {
    private readonly ICoinsQuery coinsQuery;

    public CoinsController(ICoinsQuery coinsQuery)
    {
      this.coinsQuery = coinsQuery;
    }

    [HttpGet]
    public IActionResult GetAllCoins()
    {
      try
      {
        return Ok(coinsQuery.GetAllCoins());
      }
      catch (Exception ex)
      {
        return StatusCode(500, "Could not get all products at this time.");
      }
    }
  }
}
