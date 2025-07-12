using backend.Database;
using backend.Domain;

namespace backend.Infraestructure
{
  public class CoinPaymentRepository : ICoinPaymentRepository
  {
    private readonly VendingMachineDB vendingMachineDB;

    public CoinPaymentRepository(VendingMachineDB vendingMachineDB)
    {
      this.vendingMachineDB = vendingMachineDB;
    }

    public void ProcessCoins(List<CoinPaymentModel> coins)
    {
      var vendingMachineMoney = vendingMachineDB.vendingMachineMoney;

      foreach (var coinPayment in coins)
      {
        CoinDenomination denomination = (CoinDenomination) coinPayment.Value;
        vendingMachineMoney[denomination].Quantity -= coinPayment.Quantity;
      }
    }

    public Dictionary<CoinDenomination, VendingMachineMoneyModel>
      GetAvailableVendingMachineCoins()
    {
      return vendingMachineDB.vendingMachineMoney;
    }
  }
}
