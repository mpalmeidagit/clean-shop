using AutoMapper;
using CleanShop.Application.DTO;
using CleanShop.Application.Interface;
using CleanShop.Application.Validator;
using CleanShop.Domain.Entity;
using CleanShop.Domain.Interface;
using CleanShop.Transversal.Common;
using CleanShop.Transversal.Logging;
using FluentValidation.Results;

namespace CleanShop.Application.Main;

public class AuthApplication : IAuthApplication
{
    private readonly IUsersDomain _usersDomain;
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;
    private readonly IAppLogger<AuthApplication> _logger;

    private readonly SignInDtoValidator _signInDtoValidator;
    private readonly SignUpDtoValidator _signUpDtoValidator;

    public AuthApplication(
        IUsersDomain usersDomain,
        IJwtService jwtService,
        IMapper mapper,
        IAppLogger<AuthApplication> logger,
        SignInDtoValidator signInDtoValidator,
        SignUpDtoValidator signUpDtoValidator)
    {
        _usersDomain = usersDomain;
        _jwtService = jwtService;
        _mapper = mapper;
        _logger = logger;
        _signInDtoValidator = signInDtoValidator;
        _signUpDtoValidator = signUpDtoValidator;
    }

    public async Task<Response<TokenDto>> SignInAsync(SignInDto signInDto)
    {
        var response = new Response<TokenDto>();

        var validationResult = await _signInDtoValidator.ValidateAsync(signInDto);
        if (!validationResult.IsValid)
        {
            response.Message = "Falha na validação do modelo";
            response.Errors = validationResult.Errors;
            _logger.LogWarning("Falha na validação do SignIn: {Errors}", FormatErrors(validationResult));
            return response;
        }

        try
        {
            var user = await _usersDomain.GetByEmailAsync(signInDto.Email);
            if (user is null || !await _usersDomain.CheckPasswordAsync(user, signInDto.Password))
            {
                // Mesma resposta para e-mail inexistente e senha errada (evita enumeração de usuários)
                response.Message = "Credenciais inválidas";
                _logger.LogWarning("Falha de autenticação para {Email}: {Motivo}",
                    signInDto.Email, user is null ? "e-mail não cadastrado" : "senha incorreta");
                return response;
            }

            response.Data = new TokenDto
            {
                AccessToken = _jwtService.GenerateToken(user),
                ExpiresIn = 3600
            };
            response.IsSuccess = true;
            response.Message = "Autenticação bem-sucedida";
            _logger.LogInformation("Autenticação bem-sucedida para o usuário: {Email}", signInDto.Email);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Erro inesperado no SignIn para {Email}", signInDto.Email);
            response.Message = "Não foi possível concluir a autenticação. Tente novamente.";
        }

        return response;
    }

    public async Task<Response<bool>> SignUpAsync(SignUpDto signUpDto)
    {
        var response = new Response<bool>();

        var validationResult = await _signUpDtoValidator.ValidateAsync(signUpDto);
        if (!validationResult.IsValid)
        {
            response.Message = "Falha na validação do modelo";
            response.Errors = validationResult.Errors;
            _logger.LogWarning("Falha na validação do SignUp: {Errors}", FormatErrors(validationResult));
            return response;
        }

        try
        {
            var existingUser = await _usersDomain.GetByEmailAsync(signUpDto.Email);
            if (existingUser is not null)
            {
                response.Message = "O usuário já existe.";
                _logger.LogWarning("Cadastro recusado, e-mail já cadastrado: {Email}", signUpDto.Email);
                return response;
            }

            var user = _mapper.Map<User>(signUpDto);
            response.Data = await _usersDomain.CreateUserAsync(user, signUpDto.Password);

            if (response.Data)
            {
                response.IsSuccess = true;
                response.Message = "Usuário criado com sucesso";
                _logger.LogInformation("Usuário criado com sucesso: {Email}", signUpDto.Email);
            }
            else
            {
                response.Message = "Não foi possível criar o usuário.";
                _logger.LogWarning("Falha ao criar o usuário: {Email}", signUpDto.Email);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Erro inesperado no SignUp para {Email}", signUpDto.Email);
            response.Message = "Não foi possível concluir o cadastro. Tente novamente.";
        }

        return response;
    }

    private static string FormatErrors(ValidationResult validationResult)
        => string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
}
