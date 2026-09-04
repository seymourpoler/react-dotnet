using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Tecnyfarma.Server.User.Application.LogIn;
using Tecnyfarma.Server.User.Application.Register;
using Tecnyfarma.Server.User.Domain;

namespace Tecnyfarma.Server.User.Infrastructure.DataBase;

public class SqliteUserRepository(UsersDbContext dbContext) : FindUserRepository, SaveUserRepository
{
    public async Task<Either<Error, Unit>> SaveAsync(Domain.User user)
    {
        var databaseUser = new Models.User
        {
            Id = user.Id,
            Email = user.Email.Value,
            Password = user.Password.Value,
            CreatedAtUtc = user.CreatedAtUtc,
            Type = user.Type
        };
        await dbContext.Users.AddAsync(databaseUser);
        await dbContext.SaveChangesAsync();
        return Prelude.Right<Error, Unit>(Unit.Default);
    }

    public async Task<Either<Error, Domain.User>> FindAsync(Email email)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email.Value);
        if(user is null)
        {
            return Either<Error, Domain.User>.Left(new Error("User not found"));
        }
        return await Build(user);
    }
    
    private static async Task<Either<Error, Domain.User>> Build(Models.User user)
    {
        return await (
            from email in Email.Create(user.Email).ToAsync()
            from password in Password.CreateWithEncryptedValue(user.Password).ToAsync()
            select new Domain.User(email, password)
        );
    }
}