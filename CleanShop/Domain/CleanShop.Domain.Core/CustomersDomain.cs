using CleanShop.Domain.Entity;
using CleanShop.Domain.Interface;
using CleanShop.Infrastructure.Interface;

namespace CleanShop.Domain.Core;

public class CustomersDomain : ICustomersDomain
{
    private readonly IUnitOfWork _unitOfWork;

    public CustomersDomain(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> DeleteAsync(string customerId)
    {
        return await _unitOfWork.Customers.DeleteAsync(customerId);
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        return await _unitOfWork.Customers.GetAllAsync();
    }

    public async Task<Customer> GetAsync(string customerId)
    {
        return await GetAsync(customerId);
    }

    public async Task<bool> InsertAsync(Customer customer)
    {
        return await _unitOfWork.Customers.InsertAsync(customer);
    }

    public async Task<bool> UpdateAsync(Customer customer)
    {
        return await _unitOfWork.Customers.UpdateAsync(customer);
    }
}
