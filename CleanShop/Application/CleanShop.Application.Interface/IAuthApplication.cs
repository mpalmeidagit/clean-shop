using CleanShop.Application.DTO;
using CleanShop.Transversal.Common;

namespace CleanShop.Application.Interface;

public interface IAuthApplication
{
    Task<Response<bool>> SignUpAsync(SignUpDto signUpDto);
    Task<Response<TokenDto>> SignInAsync(SignInDto signInDto);
}