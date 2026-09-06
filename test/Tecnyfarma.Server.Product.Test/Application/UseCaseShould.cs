using NSubstitute;
using Tecnyfarma.Server.Product.Application;

namespace Tecnyfarma.Server.Product.Test.Application;

public class UseCaseShould
{
    private readonly Repository repository;
    private readonly UseCase useCase;

    public UseCaseShould()
    {
        repository = Substitute.For<Repository>();
        useCase = new UseCase(repository);
    }
}