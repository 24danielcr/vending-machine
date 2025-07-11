using backend.Database;

namespace backend.Infraestructure
{
  public class CoinsRepository : ICoinsRepository
  {

    private readonly VendingMachineDB vendingMachineDB;

    public CoinsRepository(VendingMachineDB vendingMachineDB)
    {
      this.vendingMachineDB = vendingMachineDB;
    }

    public Dictionary<CoinDenomination, VendingMachineMoneyModel> GetAllCoins()
    {
      try
      {
        return vendingMachineDB.vendingMachineMoney;
      }
      catch (Exception ex)
      {
        throw new Exception(
          "Error in repository layer: GetAllCoins.", ex);
      }
    }
  }
}
