using System.Text.RegularExpressions;

namespace backend.Database
{
  public class VendingMachineDB
  {
    public readonly Dictionary<Product, VendingMachineProductModel>
      vendingMachineProducts;

    public readonly Dictionary<CoinDenomination, VendingMachineMoneyModel>
      vendingMachineMoney;

    public VendingMachineDB()
    {
      vendingMachineProducts = new Dictionary<Product
        , VendingMachineProductModel>();

      InsertProducts();

      vendingMachineMoney = new Dictionary<CoinDenomination
        , VendingMachineMoneyModel>();

      InsertCoins();
    }

    private void InsertProduct(Product product, string name, double price
      , int quantity)
    {
      vendingMachineProducts[product] = new VendingMachineProductModel
      {
        Name = name,
        Price = price,
        Quantity = quantity
      };
    }

    private string FormatProductName(Product product)
    {
      string name = product.ToString();
      return Regex.Replace(name, "(?<!^)([A-Z])", " $1");
    }

    private void InsertProducts()
    {
      var cocaColaName = FormatProductName(Product.CocaCola);
      InsertProduct(Product.CocaCola, cocaColaName, 800, 10);

      var pepsiName = FormatProductName(Product.Pepsi);
      InsertProduct(Product.Pepsi, pepsiName, 750, 8);

      var spriteName = FormatProductName(Product.CocaCola);
      InsertProduct(Product.Sprite, spriteName, 975, 15);

      var fantaName = FormatProductName(Product.CocaCola);
      InsertProduct(Product.Fanta, fantaName, 950, 10);
    }

    private void InsertCoin(CoinDenomination Denomination, int Value, int Quantity)
    {
      vendingMachineMoney[Denomination] = new VendingMachineMoneyModel
      {
        Denomination = Denomination,
        Value = Value,
        Quantity = Quantity,
      };
    }

    private int ConvertDenominationToValue(CoinDenomination denomination)
    {
      return (int)denomination;
    }

    private void InsertCoins()
    {
      InsertCoin(CoinDenomination.Colon25
        , ConvertDenominationToValue(CoinDenomination.Colon25), 25);

      InsertCoin(CoinDenomination.Colon50
        , ConvertDenominationToValue(CoinDenomination.Colon50), 50);

      InsertCoin(CoinDenomination.Colon100
        , ConvertDenominationToValue(CoinDenomination.Colon100), 30);

      InsertCoin(CoinDenomination.Colon500
        , ConvertDenominationToValue(CoinDenomination.Colon500), 20);
    }
  }
}
