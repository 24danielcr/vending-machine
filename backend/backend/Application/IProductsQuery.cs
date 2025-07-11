using backend.Database;

namespace backend.Application
{
  public interface IProductsQuery
  {
    public List<VendingMachineProductModel> GetAllProducts();
  }
}
