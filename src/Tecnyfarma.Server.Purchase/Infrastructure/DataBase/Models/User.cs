using Tecnyfarma.Server.Purchase.Domain;

namespace Tecnyfarma.Server.Purchase.Infrastructure.DataBase.Models;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public UserType Type { get; set; }
}