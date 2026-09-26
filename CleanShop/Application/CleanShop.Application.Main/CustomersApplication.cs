using AutoMapper;
using CleanShop.Application.DTO;
using CleanShop.Application.Interface;
using CleanShop.Domain.Entity;
using CleanShop.Domain.Interface;
using CleanShop.Transversal.Common;

namespace CleanShop.Application.Main;

public class CustomersApplication : ICustomersApplication
{
    private readonly ICustomersDomain _customersDomain;
    private readonly IMapper _mapper;

    public CustomersApplication(ICustomersDomain customersDomain, IMapper mapper)
    {
        _customersDomain = customersDomain;
        _mapper = mapper;
    }

    public async Task<Response<bool>> InsertAsync(CustomerDto customersDto)
    {
        var response = new Response<bool>();
        try
        {
            var customer = _mapper.Map<Customer>(customersDto);
            response.Data = await _customersDomain.InsertAsync(customer);
            if (response.Data)
            {
                response.IsSuccess = true;
                response.Message = "Registro concluído com sucesso.";
            }
        }
        catch (Exception e)
        {
            response.Message = e.Message;
        }

        return response;
    }

    public async Task<Response<bool>> UpdateAsync(CustomerDto customersDto)
    {
        var response = new Response<bool>();
        try
        {
            var customer = _mapper.Map<Customer>(customersDto);
            response.Data = await _customersDomain.UpdateAsync(customer);
            if (response.Data)
            {
                response.IsSuccess = true;
                response.Message = "Atualização concluída com sucesso.";
            }
        }
        catch (Exception e)
        {
            response.Message = e.Message;
        }

        return response;
    }

    public async Task<Response<bool>> DeleteAsync(string customerId)
    {
        var response = new Response<bool>();
        try
        {
            response.Data = await _customersDomain.DeleteAsync(customerId);
            if (response.Data)
            {
                response.IsSuccess = true;
                response.Message = "Eliminação bem-sucedida.";
            }
        }
        catch (Exception e)
        {
            response.Message = e.Message;
        }

        return response;
    }

    public async Task<Response<CustomerDto>> GetAsync(string customerId)
    {
        var response = new Response<CustomerDto>();
        try
        {
            var customer = await _customersDomain.GetAsync(customerId);
            response.Data = _mapper.Map<CustomerDto>(customer);
            if (response.Data != null)
            {
                response.IsSuccess = true;
                response.Message = "Consulta bem-sucedida.";
            }
        }
        catch (Exception e)
        {
            response.Message = e.Message;
        }

        return response;
    }

    public async Task<Response<IEnumerable<CustomerDto>>> GetAllAsync()
    {
        var response = new Response<IEnumerable<CustomerDto>>();
        try
        {
            var customers = await _customersDomain.GetAllAsync();
            response.Data = _mapper.Map<IEnumerable<CustomerDto>>(customers);
            if (response.Data != null)
            {
                response.IsSuccess = true;
                response.Message = "Consulta bem-sucedida.";
            }
        }
        catch (Exception e)
        {
            response.Message = e.Message;
        }

        return response;
    }
}