using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Tecnyfarma.Server.Purchase.Application.Product;
using Tecnyfarma.Server.Purchase.Domain;

namespace Tecnyfarma.Server.Purchase.Infrastructure.DataBase;

public class SqliteProductRepository(DbContext dbContext) : ProductRepository
{
    public async Task<Either<Error, Domain.Product>> FindProductAsync(Guid idProduct)
    {
        try
        {
            var product = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == idProduct);
            if (product == null)
            {
                return new Error("Product not found");
            }

            return Domain.Product.Create(product.Id, product.Name, product.Price);
        }
        catch (Exception exception)
        {
            return new Error(exception.Message);
        }
    }

    public async Task<Either<Error, Unit>> SaveProductAsync(Domain.Product product)
    {
        try
        {
            var dbProduct = new Tecnyfarma.Server.Purchase.Infrastructure.DataBase.Models.Product
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            };
            await dbContext.AddAsync(dbProduct);
            await dbContext.SaveChangesAsync();
            return Unit.Default;
        }
        catch(Exception exception)
        {
            return new Error(exception.Message);
        }
    }
}