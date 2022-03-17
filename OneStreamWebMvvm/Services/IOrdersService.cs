
namespace OneStreamWebMvvm
{
    public interface IOrdersService
    {
        Task<IEnumerable<Order>>? GetOrders();
        Task<IEnumerable<OrderModel>>? GetOrderModels();
    }
}
