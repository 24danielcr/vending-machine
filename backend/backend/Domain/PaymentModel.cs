using System.ComponentModel.DataAnnotations;

namespace backend.Domain
{
  public class ProductPaymentModel
  {
    [Required] public string Name { get; set; } = string.Empty;
    [Required] public int Quantity { get; set; }
  }

  public class CoinPaymentModel
  {
    [Required] public int Value { get; set; }
    [Required] public int Quantity { get; set; }
  }

  public class PaymentRequestModel
  {
    [Required] public List<CoinPaymentModel> CoinsPayment { get; set; }
    [Required] public List<ProductPaymentModel> Products { get; set; }
  }
}
