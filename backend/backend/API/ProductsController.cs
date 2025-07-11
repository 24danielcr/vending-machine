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
      return Ok(productsQuery.GetAllProducts());
    }
  }

}
