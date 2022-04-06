namespace OneStreamWebMvvm
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductModel>> GetProducts();
    }
}
