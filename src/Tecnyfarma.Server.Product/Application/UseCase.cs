namespace Tecnyfarma.Server.Product.Application;

public class UseCase(Repository repository)
{
    public virtual async Task<Result> ExecuteAsync(Args args)
    {
        var products = await repository.FindProductsAsync(args.Email);
        throw new  NotImplementedException();
    }
}