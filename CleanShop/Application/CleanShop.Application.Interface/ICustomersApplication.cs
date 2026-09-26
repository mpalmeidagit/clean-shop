using CleanShop.Application.DTO;
using CleanShop.Transversal.Common;

namespace CleanShop.Application.Interface;

public interface ICustomersApplication
{
    Task<Response<bool>> InsertAsync(CustomerDto customersDto);
    Task<Response<bool>> UpdateAsync(CustomerDto customersDto);
    Task<Response<bool>> DeleteAsync(string customerId);
    Task<Response<CustomerDto>> GetAsync(string customerId);
    Task<Response<IEnumerable<CustomerDto>>> GetAllAsync();
}