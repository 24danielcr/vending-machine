using backend.Application;
using Microsoft.AspNetCore.Mvc;

namespace backend.API
{
  [ApiController]
  [Route("api/[controller]")]
  public class ProductsController : ControllerBase
  {
    private readonly IProductsQuery productsQuery;

    public ProductsController(IProductsQuery productsQuery)
    {
      this.productsQuery = productsQuery;
    }

    [HttpGet]
    public IActionResult GetAllProducts()
    {
      try
      {
        return Ok(productsQuery.GetAllProducts());
      }
      catch (Exception ex)
      {
        return StatusCode(500, "Could not get all products at this time.");
      }
    }
  }

}
