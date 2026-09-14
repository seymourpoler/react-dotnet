using Tecnyfarma.Server.Product.Domain;

namespace Tecnyfarma.Server.Product.Infrastructure.DataBase.Models;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public UserType Type { get; set; }
}