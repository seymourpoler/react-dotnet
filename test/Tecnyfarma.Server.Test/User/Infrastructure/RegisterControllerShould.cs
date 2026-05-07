using FakeItEasy;
using Tecnyfarma.Server.User.Application;
using Tecnyfarma.Server.User.Infrastructure;

namespace Tecnyfarma.Server.Test.User.Infrastructure;

public class RegisterControllerShould
{
    private readonly UserRepository repository;
    private readonly RegisterController controller;

    public RegisterControllerShould()
    {
        repository = A.Fake<UserRepository>();
        var useCase = new RegisterUserUseCase(repository);
        controller = new RegisterController(useCase);
    }
    
    [Fact]
    public async Task RegisterUser()
    {
        var args = new RegisterUserRequest { Email = "John@Doe.dev", Password = "a-password" };
        
        var result = await controller.Register(args);
        
        
    }
}