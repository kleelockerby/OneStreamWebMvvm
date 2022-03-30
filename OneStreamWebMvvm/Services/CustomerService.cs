using System.Net.Http.Json;

namespace OneStreamWebMvvm
{
    public class CustomerService : ICustomerService
    {
        private readonly HttpClient httpClient;

        public CustomerService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public Task<IEnumerable<CustomerModel>>? GetCustomerModels()
        {
            return httpClient.GetFromJsonAsync<IEnumerable<CustomerModel>>("sample-data/customer.json")!;
        }
    }
}
