
namespace OneStreamWebMvvm
{
	public interface ICustomerService
	{
        Task<IEnumerable<CustomerModel>>? GetCustomerModels();
    }
}
