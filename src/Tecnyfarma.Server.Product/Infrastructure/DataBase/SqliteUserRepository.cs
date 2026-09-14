using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Tecnyfarma.Server.Product.Application.User;
using Tecnyfarma.Server.Product.Domain;

namespace Tecnyfarma.Server.Product.Infrastructure.DataBase;

public class SqliteUserRepository(DbContext dbContext) : UserRepository
{
    public async Task<Either<Error, Tecnyfarma.Server.Product.Domain.User>> FindUserAsync(string email)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        if(user == null)
        {
            return new Tecnyfarma.Server.Product.Domain.User(string.Empty, UserType.Freemium);
        }

        return new Tecnyfarma.Server.Product.Domain.User(user.Email, user.Type);
    }

    public async Task<Either<Error, Unit>> SaveUserAsync(Domain.User user)
    {
        await dbContext.Users.AddAsync(new Models.User { Email = user.Email, Type = user.Type });
        await dbContext.SaveChangesAsync();

        return Unit.Default;
    }
}