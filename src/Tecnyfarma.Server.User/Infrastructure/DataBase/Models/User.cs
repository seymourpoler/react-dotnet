namespace Tecnyfarma.Server.User.Infrastructure.DataBase.Models;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime CreatedAtUtc { get;  set; }
    public Domain.Type Type { get; set; }
}