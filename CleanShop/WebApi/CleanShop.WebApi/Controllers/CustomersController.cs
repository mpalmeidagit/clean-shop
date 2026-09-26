using CleanShop.Application.DTO;
using CleanShop.Application.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CleanShop.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly ICustomersApplication _customersApplication;

    public CustomersController(ICustomersApplication customersApplication)
    {
        _customersApplication = customersApplication;
    }

    [HttpPost("InsertAsync")]
    public async Task<IActionResult> InsertAsync([FromBody] CustomerDto customerDto)
    {
        if (customerDto == null)
            return BadRequest();

        var response = await _customersApplication.InsertAsync(customerDto);

        if (response.IsSuccess)
            return Ok(response);

        return StatusCode((int)HttpStatusCode.InternalServerError, response);
    }

    [HttpPut("UpdateAsync/{customerId}")]
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

    [HttpDelete("DeleteAsync/{customerId}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] string customerId)
    {
        if (string.IsNullOrEmpty(customerId))
            return BadRequest();

        var response = await _customersApplication.DeleteAsync(customerId);

        if (response.IsSuccess)
            return Ok(response);

        return StatusCode((int)HttpStatusCode.InternalServerError, response);
    }

    [HttpGet("GetAsync/{customerId}")]
    public async Task<IActionResult> GetAsync([FromRoute] string customerId)
    {
        if (string.IsNullOrEmpty(customerId))
            return BadRequest();

        var response = await _customersApplication.GetAsync(customerId);

        if (response.IsSuccess)
            return Ok(response);

        return StatusCode((int)HttpStatusCode.InternalServerError, response);
    }

    [HttpGet("GetAllAsync")]
    public async Task<IActionResult> GetAllAsync()
    {
        var response = await _customersApplication.GetAllAsync();

        if (response.IsSuccess)
            return Ok(response);

        return StatusCode((int)HttpStatusCode.InternalServerError, response);
    }
}
