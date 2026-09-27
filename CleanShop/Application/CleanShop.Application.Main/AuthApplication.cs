using AutoMapper;
using CleanShop.Application.DTO;
using CleanShop.Application.Interface;
using CleanShop.Domain.Entity;
using CleanShop.Domain.Interface;
using CleanShop.Transversal.Common;
using CleanShop.Transversal.Logging;

namespace CleanShop.Application.Main;

public class AuthApplication : IAuthApplication
{
    private readonly IUsersDomain _usersDomain;
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;
    private readonly IAppLogger<AuthApplication> _logger;

    public AuthApplication(
        IUsersDomain usersDomain, 
        IJwtService jwtService, 
        IMapper mapper, 
        IAppLogger<AuthApplication> logger)
    {
        _usersDomain = usersDomain;
        _jwtService = jwtService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Response<TokenDto>> SignInAsync(SignInDto signInDto)
    {
        var response = new Response<TokenDto>();

        try
        {
            var user = await _usersDomain.GetByEmailAsync(signInDto.Email);
            if (user == null)
            {
                response.Message = "O endereço de e-mail não existe ou não está cadastrado.";
                _logger.LogError("Falha na validação email. Error: {Message}", response.Message);
                return response;
            }

            var isValidPassword = await _usersDomain.CheckPasswordAsync(user, signInDto.Password);
            if (!isValidPassword)
            {
                response.Message = "Credenciais inválidas";
                _logger.LogError("Falha na validação senha. Error: {Message}", response.Message);
                return response;
            }

            var token = _jwtService.GenerateToken(user);
            response.Data = new TokenDto
            {
                AccessToken = token,
                ExpiresIn = 3600
            };

            response.IsSuccess = true;
            _logger.LogInformation("Autenticação bem-sucedida para o usuário: {Email}", signInDto.Email);
            response.Message = "Autenticação bem-sucedida";
        }
        catch (Exception e)
        {
            response.Message = e.Message;
        }

        return response;
    }

    public async Task<Response<bool>> SignUpAsync(SignUpDto signUpDto)
    {
        var response = new Response<bool>();
        try
        {
            var existingUser = await _usersDomain.GetByEmailAsync(signUpDto.Email);
            if (existingUser != null)
            {
                response.Message = "O usuário já existe.";
                _logger.LogError("Falha na criação do usuário. Error: {Message}", response.Message);
                return response;
            }

            var user = _mapper.Map<User>(signUpDto);
            response.Data = await _usersDomain.CreateUserAsync(user, signUpDto.Password);

            if (response.Data)
            {
                response.IsSuccess = true;
                _logger.LogInformation("Usuário criado com sucesso: {Email}", signUpDto.Email);
                response.Message = "Usuário criado com sucesso";
            }
        }
        catch (Exception e)
        {
            response.Message = e.Message;
        }

        return response;
    }
}