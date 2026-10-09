using NSubstitute;
using Tecnyfarma.Server.Product.Message;
using Tecnyfarma.Server.Purchase.Application.Product;
using Tecnyfarma.Server.Purchase.Infrastructure.EventHandler;

namespace Tecnyfarma.Server.Purchase.Test.Infrastructure.EventHandler;

public class ProductCreatedHandlerShould
{
    [Fact]
    public async Task CreateProduct()
    {
        var idProduct = Guid.NewGuid();
        var anEvent = new ProductCreated { Id = idProduct, Name =  "Test" , Price = 100 };
        var useCase = Substitute.For<CreateProductUseCase>(Substitute.For<ProductRepository>());
        var handler = new ProductCreatedHandler(useCase);
        
        await handler.Handle(anEvent);

        await useCase.Received().ExecuteAsync(Arg.Is<CreateProductArgs>(x => 
            x.IdProduct == idProduct &&
            x.Name == "Test" &&
            x.Price == 100
            )
        );
    }
}