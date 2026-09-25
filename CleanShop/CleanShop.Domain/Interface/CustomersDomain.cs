using CleanShop.Domain.Entity;

namespace CleanShop.Domain.Interface;

public interface ICustomersDomain
{
    Task<bool> InsertAsync(Customer customer);
    Task<bool> UpdateAsync(Customer customer);
    Task<bool> DeleteAsync(string customerId);
    Task<Customer> GetAsync(string customerId);
    Task<IEnumerable<Customer>> GetAllAsync();
}
