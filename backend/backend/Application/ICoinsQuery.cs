using backend.Database;

namespace backend.Application
{
  public interface ICoinsQuery
  {
    public List<VendingMachineMoneyModel> GetAllCoins();
  }
}
