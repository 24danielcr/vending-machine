using backend.Database;
using backend.Infraestructure;

namespace backend.Application
{
  public class CoinsQuery : ICoinsQuery
  {
    private readonly ICoinsRepository coinsRepository;

    public CoinsQuery(ICoinsRepository coinsRepository)
    {
      this.coinsRepository = coinsRepository;
    }

    public List<VendingMachineMoneyModel> GetAllCoins()
    {
      List<VendingMachineMoneyModel> coins =
        new List<VendingMachineMoneyModel>();
      try
      {
        var coinsQuery = coinsRepository.GetAllCoins();

        foreach (var coin in coinsQuery)
        {
          coins.Add(coin.Value);
        }
      }
      catch (Exception ex)
      {
        throw new Exception(
          "Error in application layer: GetAllCoins.", ex);
      }

      return coins;
    }
  }
}
