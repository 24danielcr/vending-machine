using backend.Database;

namespace backend.Infraestructure
{
  public class PaymentOrchestratorRepository : IPaymentOrchestratorRepository
  {
    private readonly VendingMachineDB vendingMachineDB;

    public PaymentOrchestratorRepository(VendingMachineDB vendingMachineDB)
    {
      this.vendingMachineDB = vendingMachineDB;
    }

    public int GetProductPrice(string productName)
    {
      var vendingMachineProducts = vendingMachineDB.vendingMachineProducts;
      if (Enum.TryParse<Product>
            (productName, true, out var productEnum))
      {
        return vendingMachineProducts[productEnum].Price;
      }
      else
      {
        throw new Exception(
          "Error in repository layer: GetProductQuantity.");
      }
    }

  }
}
