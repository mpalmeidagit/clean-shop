using CleanShop.Domain.Entity;
using CleanShop.Domain.Interface;
using CleanShop.Infrastructure.Interface;

namespace CleanShop.Domain.Core;

public class UsersDomain : IUsersDomain
{
    private readonly IUnitOfWork _unitOfWork;

    public UsersDomain(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> CheckPasswordAsync(User user, string password)
    {
        return await _unitOfWork.Users.CheckPasswordAsync(user, password);
    }

    public async Task<bool> CreateUserAsync(User user, string password)
    {
        return await _unitOfWork.Users.CreateUserAsync(user, password);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _unitOfWork.Users.GetByEmailAsync(email);
    }
}