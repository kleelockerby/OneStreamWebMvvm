
namespace OneStreamWebMvvm
{
	public interface IProductService
	{
		Task<IEnumerable<ProductModel>>? GetProductModels();
	}
}
