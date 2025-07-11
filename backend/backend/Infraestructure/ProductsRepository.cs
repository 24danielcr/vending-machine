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
      return this.vendingMachineDB.vendingMachineProducts;
    }
  }
}
