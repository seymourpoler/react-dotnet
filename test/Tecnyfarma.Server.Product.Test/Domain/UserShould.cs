using Shouldly;
using Tecnyfarma.Server.Product.Domain;
using Xunit;

namespace Tecnyfarma.Server.Product.Test.Domain;

public class UserShould
{
    [Theory]
    [InlineData(UserType.Freemium)]
    [InlineData(UserType.Premium)]
    public void KeepTheProvidedEmailAndType(UserType type)
    {
        var user = new Product.Domain.User("user@example.com", type);

        user.Email.ShouldBe("user@example.com");
        user.Type.ShouldBe(type);
    }

    [Fact]
    public void GenerateAGuidAsId()
    {
        var user = new Product.Domain.User("user@example.com", UserType.Freemium);

        user.Id.ShouldNotBe(Guid.Empty);
    }

    [Fact]
    public void GenerateADifferentIdForEachUser()
    {
        var first = new Product.Domain.User("first@example.com", UserType.Freemium);
        var second = new Product.Domain.User("second@example.com", UserType.Premium);

        first.Id.ShouldNotBe(second.Id);
    }

    [Theory]
    [InlineData(100f, 100f)]
    [InlineData(59.9f, 59.9f)]
    [InlineData(0f, 0f)]
    public void KeepThePriceForAFreemiumUser(float price, float expectedPrice)
    {
        var user = new Product.Domain.User("user@example.com", UserType.Freemium);

        var result = user.CalculatePrice(price);

        result.ShouldBe(expectedPrice);
    }

    [Theory]
    [InlineData(100f, 90f)]
    [InlineData(59.9f, 53.91f)]
    [InlineData(0f, 0f)]
    public void ApplyTenPercentDiscountForAPremiumUser(float price, float expectedPrice)
    {
        var user = new Product.Domain.User("user@example.com", UserType.Premium);

        var result = user.CalculatePrice(price);

        result.ShouldBe(expectedPrice, 0.0001);
    }

    [Fact]
    public void ThrowForAnUnknownUserType()
    {
        var user = new Product.Domain.User("user@example.com", (UserType)99);

        Should.Throw<ArgumentOutOfRangeException>(() => user.CalculatePrice(100f));
    }
}