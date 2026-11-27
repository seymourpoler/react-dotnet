using Shouldly;
using LanguageExt;
using NSubstitute;
using Tecnyfarma.Server.Purchase.Application.User;
using Tecnyfarma.Server.Purchase.Domain;
using Xunit;

namespace Tecnyfarma.Server.Purchase.Test.Application.User;

public class UseCaseShould
{
    private readonly UserRepository userRepository;
    private readonly UseCase useCase;

    public UseCaseShould()
    {
        userRepository = Substitute.For<UserRepository>();
        useCase = new UseCase(userRepository);
    }

    [Fact]
    public async Task ReturnTheErrorWhenTheUserCannotBeSaved()
    {
        userRepository.SaveUserAsync(Arg.Any<Tecnyfarma.Server.Purchase.Domain.User>()).Returns(new Error("Database error"));

        var result = await useCase.ExecuteAsync("user@example.com", UserType.Freemium);

        result.Match(
            _ => Assert.Fail("Expected an error but got a success result"),
            error => error.Message.ShouldBe("Database error")
        );
        await userRepository.Received(1).SaveUserAsync(Arg.Any<Tecnyfarma.Server.Purchase.Domain.User>());
    }
    
    [Theory]
    [InlineData(UserType.Freemium)]
    [InlineData(UserType.Premium)]
    public async Task SaveTheUserWithTheGivenType(UserType type)
    {
        userRepository.SaveUserAsync(Arg.Any<Tecnyfarma.Server.Purchase.Domain.User>()).Returns(Unit.Default);

        var result = await useCase.ExecuteAsync("user@example.com", type);

        result.Match(
            Left: _ => Assert.Fail("Expected an error but got a success"),
            Right: x => x.ShouldBeOfType<Unit>()
        );
        await userRepository.Received().SaveUserAsync(Arg.Is<Tecnyfarma.Server.Purchase.Domain.User>(
            user => user.Email == "user@example.com" && user.Type == type));
    }
}