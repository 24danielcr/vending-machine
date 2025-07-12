using backend.Domain;
using backend.Infraestructure;

namespace backend.Application
{
  public class CoinPaymentCommand : ICoinPaymentCommand
  {
    private readonly ICoinPaymentRepository coinPaymentRepository;

    public CoinPaymentCommand(ICoinPaymentRepository coinPaymentRepository)
    {
      this.coinPaymentRepository = coinPaymentRepository;
    }

    public bool GetPaymentChange(List<CoinPaymentModel> paymentChange
      , int totalRemainder)
    {
      var vendingMachineMoney
        = coinPaymentRepository.GetAvailableVendingMachineCoins();

      int remaining = totalRemainder;

      var sortedCoins = vendingMachineMoney
          .OrderByDescending(kv => (int)kv.Key)
          .ToList();

      foreach (var (denomination, model) in sortedCoins)
      {
        int coinValue = (int)denomination;
        int needed = remaining / coinValue;

        if (needed == 0) continue;

        int available = model.Quantity;
        int toUse = Math.Min(needed, available);

        if (toUse > 0)
        {
          paymentChange.Add(new CoinPaymentModel
          {
            Value = coinValue,
            Quantity = toUse
          });

          remaining -= toUse * coinValue;
        }

        if (remaining == 0)
          break;
      }

      if (remaining > 0)
      {
        paymentChange.Clear();
        return false;
      }

      return true;
    }

    public List<CoinPaymentModel> ProcessCoins(
      List<CoinPaymentModel> coinsPayment, int totalRemainder)
    {
      List<CoinPaymentModel> paymentChange = new List<CoinPaymentModel>();

      try
      {
        if (coinsPayment != null)
        {
          var success = GetPaymentChange(paymentChange, totalRemainder);

          if (!success)
          {
            throw new Exception(
              "Error in application layer: ProcessCoins.");
          }

          coinPaymentRepository.ProcessCoins(paymentChange);
        }
      }
      catch (Exception)
      {
        throw;
      }
       
      return paymentChange;
    }
  }
}
