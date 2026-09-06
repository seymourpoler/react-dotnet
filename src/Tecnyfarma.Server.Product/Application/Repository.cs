namespace Tecnyfarma.Server.Product.Application;

public interface Repository
{
    Task<Result> FindProductsAsync(string email);
}