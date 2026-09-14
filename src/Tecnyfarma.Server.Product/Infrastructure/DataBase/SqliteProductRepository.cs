using LanguageExt;
using Tecnyfarma.Server.Product.Application.Poduct;
using Tecnyfarma.Server.Product.Domain;
    
namespace Tecnyfarma.Server.Product.Infrastructure.DataBase;

public class SqliteProductRepository(DbContext dbContext) : ProductRepository
{
   public async Task<Either<Error, List<Domain.Product>>> FindProductsAsync()
    {
        var products = dbContext.Products.Select(p => new Domain.Product(p.Id, p.Name, p.Description, p.Price)).ToList();
        return products.Select(p => new Domain.Product(p.Id, p.Name, p.Description, p.Price)).ToList();
    }
}