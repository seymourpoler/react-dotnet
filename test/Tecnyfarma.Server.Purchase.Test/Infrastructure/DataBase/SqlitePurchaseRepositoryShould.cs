using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Shouldly;
using Tecnyfarma.Server.Purchase.Domain;
using Tecnyfarma.Server.Purchase.Infrastructure.DataBase;

namespace Tecnyfarma.Server.Purchase.Test.Infrastructure.DataBase;

public class SqlitePurchaseRepositoryShould : IDisposable
{
    private const string PurchaseEmail = "user@example.com";
    private const float ProductPrice = 100f;

    private readonly InMemoryDatabaseRoot databaseRoot = new();
    private readonly string databaseName = Guid.NewGuid().ToString();
    private readonly DbContextOptions<Tecnyfarma.Server.Purchase.Infrastructure.DataBase.DbContext> options;
    private readonly Tecnyfarma.Server.Purchase.Infrastructure.DataBase.DbContext dbContext;
    private readonly SqlitePurchaseRepository repository;

    public SqlitePurchaseRepositoryShould()
    {
        options = new DbContextOptionsBuilder<Tecnyfarma.Server.Purchase.Infrastructure.DataBase.DbContext>()
            .UseInMemoryDatabase(databaseName, databaseRoot)
            .Options;
        dbContext = new Tecnyfarma.Server.Purchase.Infrastructure.DataBase.DbContext(options);
        repository = new SqlitePurchaseRepository(dbContext);
    }

    [Fact]
    public async Task SavePurchase()
    {
        var purchase = GivenAPurchase();

        var result = await repository.SavePurchaseAsync(purchase);

        result.Match(
            Left: error => Assert.Fail($"Expected the purchase to be saved but got the error '{error.Message}'"),
            Right: unit => unit.ShouldBeOfType<Unit>()
        );

        var stored = await dbContext.Purchases.FirstOrDefaultAsync(p => p.Id == purchase.Id);
        stored.ShouldNotBeNull();
        stored.Email.ShouldBe(purchase.Email);
        stored.ProductId.ShouldBe(purchase.IdProduct);
        stored.Price.ShouldBe(purchase.Price);
        stored.CreatedAtUtc.ShouldBe(purchase.CreatedAtUtc);
    }

    [Fact]
    public async Task ReturnErrorWhenThePurchaseCannotBeSaved()
    {
        var purchase = GivenAPurchase();
        var failingRepository = new SqlitePurchaseRepository(new ThrowingDbContext(options));

        var result = await failingRepository.SavePurchaseAsync(purchase);

        result.Match(
            Left: error => error.Message.ShouldBe("Save failed"),
            Right: _ => Assert.Fail("Expected the save to fail but it succeeded")
        );
    }

    private static Domain.Purchase GivenAPurchase()
    {
        var user = Domain.User.Create(PurchaseEmail, UserType.Freemium).Match(
            Left: _ => throw new InvalidOperationException("User.Create should not fail"),
            Right: user => user);

        var product = Domain.Product.Create(Guid.NewGuid(), "Product", ProductPrice).Match(
            Left: _ => throw new InvalidOperationException("Product.Create should not fail"),
            Right: product => product);

        return Domain.Purchase.Create(user, product).Match(
            Left: _ => throw new InvalidOperationException("Purchase.Create should not fail"),
            Right: purchase => purchase);
    }

    public void Dispose()
    {
        dbContext.Dispose();
    }
}
