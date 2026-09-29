using CleanShop.Application.DTO;
using FluentValidation;

namespace CleanShop.Application.Validator;

public class SignInDtoValidator : AbstractValidator<SignInDto>
{
    public SignInDtoValidator()
    {
        RuleFor(s => s.Email).NotEmpty().EmailAddress();
        RuleFor(s => s.Password).NotEmpty();
    }
}
