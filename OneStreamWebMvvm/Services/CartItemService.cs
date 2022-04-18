using System.Net.Http.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneStreamWebMvvm
{
	public class CartItemService : ICartItemService
    {
        private readonly HttpClient httpClient;

        public CartItemService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public Task<IEnumerable<CartItemModel>>? GetCartItemModels()
        {
            return httpClient.GetFromJsonAsync<IEnumerable<CartItemModel>>("sample-data/cartItem.json")!;
        }
    }
}
