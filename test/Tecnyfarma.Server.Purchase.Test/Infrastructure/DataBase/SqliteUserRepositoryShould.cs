using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Shouldly;
using Tecnyfarma.Server.Purchase.Domain;
using Tecnyfarma.Server.Purchase.Infrastructure.DataBase;

namespace Tecnyfarma.Server.Purchase.Test.Infrastructure.DataBase;

public class SqliteUserRepositoryShould : IDisposable
{
    private const string UserEmail = "user@example.com";

    private readonly InMemoryDatabaseRoot databaseRoot = new();
    private readonly string databaseName = Guid.NewGuid().ToString();
    private readonly DbContextOptions<Tecnyfarma.Server.Purchase.Infrastructure.DataBase.DbContext> options;
    private readonly Tecnyfarma.Server.Purchase.Infrastructure.DataBase.DbContext dbContext;
    private readonly SqliteUserRepository repository;

    public SqliteUserRepositoryShould()
    {
        options = new DbContextOptionsBuilder<Tecnyfarma.Server.Purchase.Infrastructure.DataBase.DbContext>()
            .UseInMemoryDatabase(databaseName, databaseRoot)
            .Options;
        dbContext = new Tecnyfarma.Server.Purchase.Infrastructure.DataBase.DbContext(options);
        repository = new SqliteUserRepository(dbContext);
    }

    [Fact]
    public async Task ReturnTheUserWhenItExists()
    {
        var dbUser = new Tecnyfarma.Server.Purchase.Infrastructure.DataBase.Models.User { Id = Guid.NewGuid(), Email = UserEmail, Type = UserType.Premium };
        dbContext.Users.Add(dbUser);
        await dbContext.SaveChangesAsync();

        var result = await repository.FindUserAsync(UserEmail);

        result.Match(
            Left: error => Assert.Fail($"Expected a user but got the error '{error.Message}'"),
            Right: user =>
            {
                user.Email.ShouldBe(UserEmail);
                user.Type.ShouldBe(UserType.Premium);
            });
    }

    [Fact]
    public async Task ReturnErrorWhenTheUserDoesNotExist()
    {
        var result = await repository.FindUserAsync(UserEmail);

        result.Match(
            Right: _ => Assert.Fail("Expected an error but got a user"),
            Left: error => error.Message.ShouldBe("User not found"));
    }

    [Fact]
    public async Task ReturnErrorWhenTheLookupFails()
    {
        var result = await repository.FindUserAsync(UserEmail);

        result.Match(
            Left: error => error.Message.ShouldBe("User not found"),
            Right: _ => Assert.Fail("Expected a user but got the error")
            );
    }

    [Fact]
    public async Task PersistTheUserWhenItIsSaved()
    {
        var user = GivenAUser(UserType.Freemium);

        var result = await repository.SaveUserAsync(user);

        result.Match(
            Left: _ => Assert.Fail("Expected a user but got the error"),
            Right: x => x.ShouldBeOfType<Unit>()
        );
        var stored = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == UserEmail);
        stored.ShouldNotBeNull();
        stored!.Type.ShouldBe(UserType.Freemium);
    }

    [Fact]
    public async Task ReturnErrorWhenTheUserCannotBeSaved()
    {
        var user = GivenAUser(UserType.Freemium);
        var failingRepository = new SqliteUserRepository(new ThrowingDbContext(options));

        var result = await failingRepository.SaveUserAsync(user);

        result.Match(
            Left: error => error.Message.ShouldBe("Save failed"),
            Right: _ => Assert.Fail("Expected a user saved, but got the error")
        );
    }

    private static Domain.User GivenAUser(UserType type)
    {
        return Domain.User.Create(UserEmail, type).Match(
            Left: _ => throw new InvalidOperationException("User.Create should not fail"),
            Right: user => user
        );
    }

    public void Dispose()
    {
        dbContext.Dispose();
    }
}
