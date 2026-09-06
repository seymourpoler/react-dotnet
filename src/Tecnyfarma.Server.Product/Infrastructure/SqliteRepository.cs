using Tecnyfarma.Server.Product.Application;

namespace Tecnyfarma.Server.Product.Infrastructure;

public class SqliteRepository : Repository
{
    public Task<Result> FindProductsAsync(string email)
    {
        throw new NotImplementedException();
    }
}