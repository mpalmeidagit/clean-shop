using CleanShop.Domain.Entity;
using CleanShop.Infrastructure.Data;
using CleanShop.Infrastructure.Interface;
using Dapper;
using System.Data;

namespace CleanShop.Infrastructure.Repository;

public class CustomersRepository : ICustomersRepository
{
    private readonly DapperContext _context;

    public CustomersRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        using var connection = _context.CreateConnection();
        var query = "CustomersList";

        var customers = await connection.QueryAsync<Customer>(query, commandType: CommandType.StoredProcedure);

        return customers;
    }

    public async Task<Customer?> GetAsync(string customerId)
    {
        using var connection = _context.CreateConnection();
        var query = "CustomersGetByID";
        var parameters = new DynamicParameters();
        parameters.Add("CustomerID", customerId);

        var customer = await connection.QuerySingleAsync<Customer>(query, param: parameters, commandType: CommandType.StoredProcedure);

        return customer;
    }

    public async Task<bool> InsertAsync(Customer customer)
    {
        using var connection = _context.CreateConnection();
        var query = "CustomersInsert";
        var parameters = new DynamicParameters();
        parameters.Add("CustomerID", customer.CustomerId);
        parameters.Add("CompanyName", customer.CompanyName);
        parameters.Add("ContactName", customer.ContactName);
        parameters.Add("ContactTitle", customer.ContactTitle);
        parameters.Add("Address", customer.Address);
        parameters.Add("City", customer.City);
        parameters.Add("Region", customer.Region);
        parameters.Add("PostalCode", customer.PostalCode);
        parameters.Add("Country", customer.Country);
        parameters.Add("Phone", customer.Phone);
        parameters.Add("Fax", customer.Fax);

        var result = await connection.ExecuteAsync(query, param: parameters, commandType: CommandType.StoredProcedure);

        return result > 0;
    }

    public async Task<bool> UpdateAsync(Customer customer)
    {
        using var connection = _context.CreateConnection();
        var query = "CustomersUpdate";
        var parameters = new DynamicParameters();
        parameters.Add("CustomerID", customer.CustomerId);
        parameters.Add("CompanyName", customer.CompanyName);
        parameters.Add("ContactName", customer.ContactName);
        parameters.Add("ContactTitle", customer.ContactTitle);
        parameters.Add("Address", customer.Address);
        parameters.Add("City", customer.City);
        parameters.Add("Region", customer.Region);
        parameters.Add("PostalCode", customer.PostalCode);
        parameters.Add("Country", customer.Country);
        parameters.Add("Phone", customer.Phone);
        parameters.Add("Fax", customer.Fax);

        var result = await connection.ExecuteAsync(query, param: parameters, commandType: CommandType.StoredProcedure);

        return result > 0;
    }

    public async Task<bool> DeleteAsync(string customerId)
    {
        using var connection = _context.CreateConnection();
        var query = "CustomersDelete";
        var parameters = new DynamicParameters();
        parameters.Add("CustomerID", customerId);

        var result = await connection.ExecuteAsync(query, param: parameters, commandType: CommandType.StoredProcedure);

        return result > 0;
    }
}