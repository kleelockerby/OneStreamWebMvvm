
namespace OneStreamWebMvvm
{
    public interface IOrdersService
    {
        Task<IEnumerable<OrderModel>>? GetOrders();
    }
}
