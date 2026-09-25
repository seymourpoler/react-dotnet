using Microsoft.AspNetCore.Mvc;
using Tecnyfarma.Server.Purchase.Infrastructure;

namespace Tecnyfarma.Server.Purchase.Test.Infrastructure;

public class PurchaseControllerShould
{
    private readonly PurchaseController controller;
    
    public PurchaseControllerShould()
    {
        controller = new PurchaseController();
    }

    [Fact]
    public async Task CreatePurchase()
    {
        var result = await controller.CreatePurchase();
        
        Assert.IsType<OkResult>(result);
    }
}