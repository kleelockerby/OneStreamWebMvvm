using System.Net.Http.Json;

namespace OneStreamWebMvvm
{
	public class ProductService : IProductService
    {
        private readonly HttpClient httpClient;

        public ProductService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public Task<IEnumerable<ProductModel>>? GetProductModels()
        {
            return httpClient.GetFromJsonAsync<IEnumerable<ProductModel>>("sample-data/product.json")!;
        }
    }
}
