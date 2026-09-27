namespace CleanShop.Infrastructure.Interface;

public interface IUnitOfWork : IDisposable
{
    ICustomersRepository Customers { get; }
    IUsersRepository Users { get; }
}
