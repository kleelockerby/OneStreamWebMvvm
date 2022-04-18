using Xunit;
using Moq;

namespace OneStreamWebMvvm.Tests
{
    public class OrdersTests
    {
        private Mock<ICartItemService> cartItemServiceMock;
        private ShoppingCartViewModel? shoppingCartViewModel;

        public OrdersTests()
        {
            Mock<IProductRepository> productRepositoryMock = new Mock<IProductRepository>();
            this.cartItemServiceMock = new Mock<ICartItemService>();
            Mock<CartItemModel> cartItemModelMock = new Mock<CartItemModel>();

            List<CartItemModel>? cartItems = new List<CartItemModel>() { cartItemModelMock.Object };
            this.cartItemServiceMock.Setup(repo => repo.GetCartItemModels()).ReturnsAsync(cartItems.AsEnumerable());

            IList<ProductModel> productDetails = new List<ProductModel>()
             {
                 new ProductModel()
                 {
                    ProductID = 98,
                    Name = "Some Fake Product",
                    Description = "Some Fake Product Description",
                    Price = 9.99M
                 },
                 new ProductModel()
                 {
                    ProductID = 99,
                    Name = "Another Fake Product",
                    Description = "Another Fake Product Description",
                    Price = 8.99M
                  }
             };
            productRepositoryMock.Setup(repo => repo.GetProducts()).ReturnsAsync(productDetails.AsEnumerable());
            shoppingCartViewModel = new ShoppingCartViewModel(cartItemServiceMock.Object, productRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllProducts()
        {
            int expected = 2;
            var products = await shoppingCartViewModel?.GetProductRepository()!;
            var productsList = products.ToList();
            Assert.Equal(expected, productsList.Count());
        }
    }
}
