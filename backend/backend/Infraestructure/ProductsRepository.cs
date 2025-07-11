using backend.Database;

namespace backend.Infraestructure
{
  public class ProductsRepository : IProductsRepository
  {
    private readonly VendingMachineDB vendingMachineDB;

    public ProductsRepository(VendingMachineDB vendingMachineDB)
    {
      this.vendingMachineDB = vendingMachineDB;
    }

    public Dictionary<Product, VendingMachineProductModel> GetAllProducts()
    {
      try
      {
        return this.vendingMachineDB.vendingMachineProducts;
      }
      catch (Exception ex)
      {
        throw new Exception(
          "Error in repository layer: GetAllProducts.", ex);
      }
    }
  }
}
