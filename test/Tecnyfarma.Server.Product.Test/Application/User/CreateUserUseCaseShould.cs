using LanguageExt;
using NSubstitute;
using Shouldly;
using Tecnyfarma.Server.Product.Application.User;
using Tecnyfarma.Server.Product.Domain;
using Xunit;

namespace Tecnyfarma.Server.Product.Test.Application.User;

public class CreateUserUseCaseShould
{
    private readonly UserRepository userRepository;
    private readonly CreateUserUseCase useCase;

    public CreateUserUseCaseShould()
    {
        userRepository = Substitute.For<UserRepository>();
        useCase = new CreateUserUseCase(userRepository);
    }

    [Theory]
    [InlineData(UserType.Freemium)]
    [InlineData(UserType.Premium)]
    public async Task SaveTheUserWithTheGivenType(UserType type)
    {
        userRepository.SaveUserAsync(Arg.Any<Tecnyfarma.Server.Product.Domain.User>()).Returns(Unit.Default);

        var result = await useCase.ExecuteAsync("user@example.com", type);

        await userRepository.Received(1).SaveUserAsync(Arg.Is<Tecnyfarma.Server.Product.Domain.User>(
            user => user.Email == "user@example.com" && user.Type == type));
        result.IsRight.ShouldBeTrue();
    }

    [Fact]
    public async Task ReturnTheErrorWhenTheUserCannotBeSaved()
    {
        userRepository.SaveUserAsync(Arg.Any<Tecnyfarma.Server.Product.Domain.User>()).Returns(new Error("Database error"));

        var result = await useCase.ExecuteAsync("user@example.com", UserType.Freemium);

        result.Match(
            _ => Assert.Fail("Expected an error but got a success result"),
            error => error.Message.ShouldBe("Database error")
        );
        await userRepository.Received(1).SaveUserAsync(Arg.Any<Tecnyfarma.Server.Product.Domain.User>());
    }
}