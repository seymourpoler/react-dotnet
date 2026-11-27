using LanguageExt;
using Tecnyfarma.Server.Product.Application.Product;
using Tecnyfarma.Server.Product.Domain;
    
namespace Tecnyfarma.Server.Product.Infrastructure.DataBase;

public class SqliteProductRepository(DbContext dbContext) : Repository
{
   public async Task<Either<Error, List<Domain.Product>>> FindProductsAsync()
   {
       return dbContext.Products.Select(p => new Domain.Product(
               p.Id,
               p.Name,
               p.Description,
               p.Price))
           .ToList();
   }

    public async Task<Either<Error, Unit>> SaveAsync(Domain.Product product)
    {
        await dbContext.Products.AddAsync(new Models.Product
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price
        });
        return Unit.Default;
    }
}