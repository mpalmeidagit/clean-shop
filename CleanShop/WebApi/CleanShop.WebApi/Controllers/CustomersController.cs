using CleanShop.Application.DTO;
using CleanShop.Application.Interface;
using CleanShop.Transversal.Common;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace CleanShop.WebApi.Controllers;

/// <summary>
/// 
/// </summary>
[Route("api/[controller]")]
[ApiController]
[SwaggerTag("Transações relacionadas ao cliente")]
public class CustomersController : ControllerBase
{
    private readonly ICustomersApplication _customersApplication;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="customersApplication"></param>
    public CustomersController(ICustomersApplication customersApplication)
    {
        _customersApplication = customersApplication;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="customerDto"></param>
    /// <returns></returns>
    [HttpPost("InsertAsync")]
    [SwaggerOperation(
    Summary = "Cadastrar um cliente",
    Description = "Retorna um objeto genérico com o resultado da operação.")]
    [SwaggerResponse(200, "Cliente cadastrado", typeof(Response<bool>))]
    public async Task<IActionResult> InsertAsync([FromBody] CustomerDto customerDto)
    {
        if (customerDto == null)
            return BadRequest();

        var response = await _customersApplication.InsertAsync(customerDto);

        if (response.IsSuccess)
            return Ok(response);

        return StatusCode((int)HttpStatusCode.InternalServerError, response);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="customerId"></param>
    /// <param name="customerDto"></param>
    /// <returns></returns>
    [HttpPut("UpdateAsync/{customerId}")]
    [SwaggerOperation(
    Summary = "Atualizar um cliente com base no ID dele",
    Description = "Retorna um objeto genérico com o resultado da operação.")]
    [SwaggerResponse(200, "Cliente atualizado", typeof(Response<bool>))]
    public async Task<IActionResult> UpdateAsync([FromRoute] string customerId, [FromBody] CustomerDto customerDto)
    {
        if (customerDto == null)
            return BadRequest();

        if (!customerId.Equals(customerDto.CustomerId))
            return BadRequest();

        var response = await _customersApplication.UpdateAsync(customerDto);

        if (response.IsSuccess)
            return Ok(response);

        return StatusCode((int)HttpStatusCode.InternalServerError, response);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="customerId"></param>
    /// <returns></returns>
    [HttpDelete("DeleteAsync/{customerId}")]
    [SwaggerOperation(
    Summary = "Excluir um cliente com base no seu ID",
    Description = "Retorna um objeto genérico com o resultado da operação.")]
    [SwaggerResponse(200, "Cliente removido", typeof(Response<bool>))]
    public async Task<IActionResult> DeleteAsync([FromRoute] string customerId)
    {
        if (string.IsNullOrEmpty(customerId))
            return BadRequest();

        var response = await _customersApplication.DeleteAsync(customerId);

        if (response.IsSuccess)
            return Ok(response);

        return StatusCode((int)HttpStatusCode.InternalServerError, response);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="customerId"></param>
    /// <returns></returns>
    [HttpGet("GetAsync/{customerId}")]
    [SwaggerOperation(
    Summary = "Obter um cliente com base em seu ID",
    Description = "Retorna um objeto genérico com o resultado da operação.")]
    [SwaggerResponse(200, "Cliente encontrado", typeof(Response<CustomerDto>))]
    public async Task<IActionResult> GetAsync([FromRoute] string customerId)
    {
        if (string.IsNullOrEmpty(customerId))
            return BadRequest();

        var response = await _customersApplication.GetAsync(customerId);

        if (response.IsSuccess)
            return Ok(response);

        return StatusCode((int)HttpStatusCode.InternalServerError, response);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetAllAsync")]
    [SwaggerOperation(
    Summary = "Lista de todos os clientes",
    Description = "Retorna um objeto genérico com o resultado da operação.")]
    [SwaggerResponse(200, "Clientes encontrados", typeof(Response<IEnumerable<CustomerDto>>))]
    public async Task<IActionResult> GetAllAsync()
    {
        var response = await _customersApplication.GetAllAsync();

        if (response.IsSuccess)
            return Ok(response);

        return StatusCode((int)HttpStatusCode.InternalServerError, response);
    }
}