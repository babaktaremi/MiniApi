using System.Net;
using System.Net.Http.Json;
using Aspire.Hosting.Testing;

namespace MiniApi.IntegrationTests;

public class ApiTests
{
    [Fact]
    public async Task Create_Order_Should_Return_Ok()
    {
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.MiniApi_Hosting>();

        var app = await appHost.BuildAsync();

        await app.ResourceNotifications.WaitForResourceHealthyAsync("miniApi");

        var client =  app.CreateHttpClient("miniApi");

        var result = await client.PostAsJsonAsync("/order", new
        {
            orderName = "TestOrder"
        });
        
        Assert.Equal(HttpStatusCode.Created,result.StatusCode);
        
    }
}