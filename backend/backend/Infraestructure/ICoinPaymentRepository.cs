using backend.Database;
using backend.Domain;

namespace backend.Infraestructure
{
  public interface ICoinPaymentRepository
  {
    public void ProcessCoins(List<CoinPaymentModel> coins);

    public Dictionary<CoinDenomination, VendingMachineMoneyModel>
      GetAvailableVendingMachineCoins();
  }
}
