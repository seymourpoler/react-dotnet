using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Shouldly;
using Tecnyfarma.Server.Purchase.Infrastructure.DataBase;
using AppDbContext = Tecnyfarma.Server.Purchase.Infrastructure.DataBase.DbContext;
using DbProduct = Tecnyfarma.Server.Purchase.Infrastructure.DataBase.Models.Product;

namespace Tecnyfarma.Server.Purchase.Test.Infrastructure.DataBase;

public class SqliteProductRepositoryShould : IDisposable
{
    private const string ProductName = "Paracetamol";
    private const float ProductPrice = 12.5f;

    private readonly InMemoryDatabaseRoot databaseRoot = new();
    private readonly string databaseName = Guid.NewGuid().ToString();
    private readonly DbContextOptions<AppDbContext> options;
    private readonly AppDbContext dbContext;
    private readonly SqliteProductRepository repository;

    public SqliteProductRepositoryShould()
    {
        options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName, databaseRoot)
            .Options;

        dbContext = new AppDbContext(options);
        repository = new SqliteProductRepository(dbContext);
    }

    [Fact]
    public async Task ReturnTheProductWhenItExists()
    {
        var id = Guid.NewGuid();
        dbContext.Products.Add(new DbProduct { Id = id, Name = ProductName, Price = ProductPrice });
        await dbContext.SaveChangesAsync();

        var result = await repository.FindProductAsync(id);

        result.Match(
            Left: error => Assert.Fail($"Expected a product but got the error '{error.Message}'"),
            Right: product =>
            {
                product.Id.ShouldBe(id);
                product.Name.ShouldBe(ProductName);
                product.Price.ShouldBe(ProductPrice);
            });
    }

    [Fact]
    public async Task ReturnErrorWhenTheProductDoesNotExist()
    {
        var result = await repository.FindProductAsync(Guid.NewGuid());

        result.Match(
            Right: _ => Assert.Fail("Expected an error but got a product"),
            Left: error => error.Message.ShouldBe("Product not found"));
    }

    [Fact]
    public async Task ReturnErrorWhenTheLookupFails()
    {
        var result = await repository.FindProductAsync(Guid.NewGuid());

        result.Match(
            Left: error => error.Message.ShouldBe("Product not found"),
            Right: _ => Assert.Fail("Find product failed")
        );
    }

    [Fact]
    public async Task PersistTheProductWhenItIsSaved()
    {
        var product = GivenAProduct();

        var result = await repository.SaveProductAsync(product);

        result.Match(
            Left: _ => Assert.Fail("Save failed"),
            Right: x => x.ShouldBeOfType<Unit>()
        );
        await using var verificationContext = new AppDbContext(options);
        var stored = await verificationContext.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
        stored.ShouldNotBeNull();
        stored!.Name.ShouldBe(ProductName);
        stored.Price.ShouldBe(ProductPrice);
    }

    [Fact]
    public async Task ReturnErrorWhenTheProductCannotBeSaved()
    {
        var product = GivenAProduct();
        var failingRepository = new SqliteProductRepository(new ThrowingDbContext(options));

        var result = await failingRepository.SaveProductAsync(product);

        result.Match(
            Left: error => error.Message.ShouldBe("Save failed"),
            Right: x => x.ShouldBeOfType<Unit>()
        );
    }

    private static Domain.Product GivenAProduct()
    {
        return Domain.Product.Create(Guid.NewGuid(), ProductName, ProductPrice).Match(
            Left: _ => throw new InvalidOperationException("Product.Create should not fail"),
            Right: product => product);
    }

    public void Dispose()
    {
        dbContext.Dispose();
    }

    private sealed class ThrowingDbContext(DbContextOptions<AppDbContext> options) : AppDbContext(options)
    {
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            throw new InvalidOperationException("Save failed");
    }
}
