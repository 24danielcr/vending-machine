namespace backend.Infraestructure
{
  public interface IPaymentOrchestratorRepository
  {
    public int GetProductPrice(string productName);
  }
}
