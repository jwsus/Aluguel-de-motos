public interface IOrderRepository
{
    Task AddOrderAsync(Order order);
}