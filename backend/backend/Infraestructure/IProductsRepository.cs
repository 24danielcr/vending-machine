using backend.Database;

namespace backend.Infraestructure
{
  public interface IProductsRepository
  {
    public Dictionary<Product, VendingMachineProductModel> GetAllProducts();
  }
}
