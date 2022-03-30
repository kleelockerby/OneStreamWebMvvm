
namespace OneStreamWebMvvm
{
	public interface ICartItemService
	{
		Task<IEnumerable<CartItemModel>>? GetCartItemModels();
	}
}
