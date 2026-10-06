using CleanTeeth.API.ExceptionHandling;
using CleanTeeth.API.Infrastructure;
using CleanTeeth.Application;
using CleanTeeth.Infrastructure;
using CleanTeeth.Persistence;
using CleanTeeth.Security;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplication()
    .AddPersistence()
    .AddInfrastructure()
    .AddSecurityService(builder.Configuration);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Components ??= new();
        document.Components.SecuritySchemes ??= new Dictionary<string, Microsoft.OpenApi.Models.OpenApiSecurityScheme>();
        document.Components.SecuritySchemes.Add("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
            Scheme = "bearer",
            Description = "Enter your bearer token"
        });
        document.Info.Title = "CleanTeeth API";
        document.Info.Version = "v1";
        document.Info.Description = "基于 Minimal API 和垂直切片架构的牙科诊所管理系统";
        return Task.CompletedTask;
    });
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("CleanTeeth API Reference");
        options.WithTheme(ScalarTheme.Default);
    });
}
await SecuritySeeder.SeedAsync(app.Services);
app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapEndpoints();
app.Run();