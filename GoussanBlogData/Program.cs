using Azure.Identity;
using GoussanBlogData;
using GoussanBlogData.Hubs;
using GoussanBlogData.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;

try
{
    var builder = WebApplication.CreateBuilder(args);
    var startup = new Startup(builder.Configuration, builder.Environment);
    startup.ConfigureServices(builder.Services);

    // Configure Azure Key Vault
    var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri")!);
    builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (builder.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    // Configure Swagger
    app.UseSwagger(c =>
    {
        c.SerializeAsV2 = true;
        c.PreSerializeFilters.Add((document, request) =>
        {
            document.Servers = new List<OpenApiServer> { new OpenApiServer { Url = request.Host.Value } };
        });
    });
    // Need Use Cors for SignalR to function properly
    app.UseCors("ClientPermission");
    app.UseHttpsRedirection();
    // Add Jwt Token Middleware
    app.UseMiddleware<JwtMiddleware>();

    app.UseRouting();
    app.UseAuthorization();
    // Configure Endpoints
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
        endpoints.MapHub<ChatHub>("/chathub");
    });

    app.Run();
}
catch (Exception)
{
    throw;
}