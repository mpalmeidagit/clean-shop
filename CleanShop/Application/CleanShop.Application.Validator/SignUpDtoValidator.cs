using CleanShop.Application.DTO;
using FluentValidation;

namespace CleanShop.Application.Validator;

public class SignUpDtoValidator : AbstractValidator<SignUpDto>
{
    public SignUpDtoValidator()
    {
        RuleFor(s => s.FirstName).NotEmpty();
        RuleFor(s => s.LastName).NotEmpty();
        RuleFor(s => s.UserName).NotEmpty();
        RuleFor(s => s.Email).NotEmpty().EmailAddress();
        RuleFor(s => s.Password).NotEmpty();
    }
}
