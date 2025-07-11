using backend.Database;

namespace backend.Infraestructure
{
  public interface ICoinsRepository
  {
    public Dictionary<CoinDenomination, VendingMachineMoneyModel> GetAllCoins();
  }
}
