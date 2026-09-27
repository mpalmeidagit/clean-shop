using CleanShop.Domain.Entity;

namespace CleanShop.Transversal.Common;

public interface IJwtService
{
    string GenerateToken(User user);
}