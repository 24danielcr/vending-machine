using backend.Database;
using backend.Infraestructure;

namespace backend.Application
{
  public class ProductsQuery : IProductsQuery
  {
    private readonly IProductsRepository productsRepository;

    public ProductsQuery(IProductsRepository productsRepository)
    {
      this.productsRepository = productsRepository;
    }

    public List<VendingMachineProductModel> GetAllProducts()
    {
      List<VendingMachineProductModel> products =
        new List<VendingMachineProductModel>();
      try
      {
        var productsQuery = productsRepository.GetAllProducts();

        foreach (var product in productsQuery)
        {
          products.Add(product.Value);
        }
      }
      catch (Exception ex)
      {
        throw new Exception(
          "Error in application layer: GetAllProducts.", ex);
      }
      return products;
    }
  }
}
