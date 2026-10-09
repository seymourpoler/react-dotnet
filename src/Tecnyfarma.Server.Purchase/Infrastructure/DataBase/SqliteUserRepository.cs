using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Tecnyfarma.Server.Purchase.Application.User;
using Tecnyfarma.Server.Purchase.Domain;

namespace Tecnyfarma.Server.Purchase.Infrastructure.DataBase;

public class SqliteUserRepository(DbContext dbContext) : UserRepository
{
    public async Task<Either<Error, Domain.User>> FindUserAsync(string email)
    {
        try
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return new Error("User not found");
            }

            return Domain.User.Create(user.Email, user.Type);
        }
        catch (Exception exception)
        {
            return new Error(exception.Message);
        }
    }

    public async Task<Either<Error, Unit>> SaveUserAsync(Domain.User user)
    {
        try
        {
            var dbUser = new Models.User
            {
                Id = user.Id,
                Email =  user.Email,
                Type = user.Type
            };
            await  dbContext.AddAsync(dbUser);
            await dbContext.SaveChangesAsync();
            return Unit.Default;
        }
        catch (Exception exception)
        {
            return new  Error(exception.Message);
        }
    }
}