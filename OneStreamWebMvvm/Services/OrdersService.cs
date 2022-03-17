using System.Net.Http.Json;

namespace OneStreamWebMvvm
{
    public class OrdersService : IOrdersService
    {
        private readonly HttpClient httpClient;

        public OrdersService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public Task<IEnumerable<Order>>? GetOrders()
        {
            return httpClient.GetFromJsonAsync<IEnumerable<Order>>("sample-data/order.json")!;
        }

        public Task<IEnumerable<OrderModel>>? GetOrderModels()
        {
            return httpClient.GetFromJsonAsync<IEnumerable<OrderModel>>("sample-data/order.json")!;
        }
    }
}
