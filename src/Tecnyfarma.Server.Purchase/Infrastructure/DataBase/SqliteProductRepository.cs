using LanguageExt;
using Tecnyfarma.Server.Purchase.Application.Product;
using Tecnyfarma.Server.Purchase.Domain;

namespace Tecnyfarma.Server.Purchase.Infrastructure.DataBase;

public class SqliteProductRepository : ProductRepository
{
    public Task<Either<Error, Domain.Product>> FindProductAsync(Guid idProduct)
    {
        throw new NotImplementedException();
    }

    public Task<Either<Error, Unit>> SaveProductAsync(Domain.Product product)
    {
        throw new NotImplementedException();
    }
}