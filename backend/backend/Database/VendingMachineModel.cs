using System.ComponentModel.DataAnnotations;

namespace backend.Database
{
  [Flags]
  public enum Product
  {
    CocaCola = 1,
    Pepsi = 2,
    Sprite = 4,
    Fanta = 8,
  }

  public enum CoinDenomination
  {
    Colon25 = 25,
    Colon50 = 50,
    Colon100 = 100,
    Colon500 = 500
  } 

  public class VendingMachineProductModel
  {
    [Required] public string Name { get; set; } = string.Empty;
    [Required] public int Price { get; set; }
    [Required] public int Quantity { get; set; }
  }

  public class VendingMachineMoneyModel
  {
    [Required] public CoinDenomination Denomination { get; set; }
    [Required] public int Value { get; set; }
    [Required] public int Quantity { get; set; }
  }
}