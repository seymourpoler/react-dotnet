namespace Tecnyfarma.Server.User.Application.Purchase.Create;

public sealed record Args(string Email, string Name, float Price, DateTime CreatedAtUtc)
{
}