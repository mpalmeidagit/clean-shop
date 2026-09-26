namespace CleanShop.Infrastructure.Interface;

public interface IUnitOfWork : IDisposable
{
    ICustomersRepository Customers { get; }
}
