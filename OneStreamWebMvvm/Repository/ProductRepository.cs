namespace OneStreamWebMvvm
{
    public class ProductRepository : IProductRepository
    {
        private readonly IProductService ProductService;

        public ProductRepository(IProductService productService)
        {
            this.ProductService = productService;
        }

        public async Task<IEnumerable<ProductModel>> GetProducts()
        {
            IEnumerable<ProductModel> productModels = await ProductService.GetProductModels()!;
            return productModels;
        }
    }
}
