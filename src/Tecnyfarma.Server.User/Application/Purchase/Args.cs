namespace Tecnyfarma.Server.User.Application.Purchase;

public sealed record Args(string Email, string Name, float Price, DateTime CreatedAtUtc)
{
}