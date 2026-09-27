using CleanShop.Application.DTO;
using CleanShop.Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CleanShop.WebApi.Controllers;

/// <summary>
/// 
/// </summary>
/// 

[Authorize]
[Route("api/[controller]")]
[ApiController]
[SwaggerTag("Operações de autenticação")]
public class AuthController : ControllerBase
{
    private readonly IAuthApplication _authApplication;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="authApplication"></param>
    public AuthController(IAuthApplication authApplication)
    {
        _authApplication = authApplication;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="signUpDto"></param>
    /// <returns></returns>
    /// 
    [AllowAnonymous]
    [HttpPost("SignUp")]
    [SwaggerOperation(Summary = "Cadastre um novo usuário")]
    public async Task<IActionResult> SignUpAsync([FromBody] SignUpDto signUpDto)
    {
        var response = await _authApplication.SignUpAsync(signUpDto);

        if (response.IsSuccess)
            return Ok(response);

        return BadRequest(response);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="signInDto"></param>
    /// <returns></returns>
    /// 
    [AllowAnonymous]
    [HttpPost("SignIn")]
    [SwaggerOperation(Summary = "Autenticar um usuário e gerar um token.")]
    public async Task<IActionResult> SignInAsync([FromBody] SignInDto signInDto)
    {
        var response = await _authApplication.SignInAsync(signInDto);

        if (response.IsSuccess)
            return Ok(response);

        return Unauthorized(response);
    }
}