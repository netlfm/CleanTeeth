using CleanTeeth.API.ExceptionHandling;
using CleanTeeth.API.Infrastructure;
using CleanTeeth.Application;
using CleanTeeth.Persistence;
using Scalar.AspNetCore;             // 引入 Scalar 扩展方法

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. 服务注册阶段 (Service Registration)
// ==========================================

// 注册业务分层服务
builder.Services.AddApplication()
    .AddPersistence();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();// 注册 API 探索器，用于收集端点元数据
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddFluentResults();
// 注册 .NET 9 原生的 OpenAPI 文档生成服务
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Title = "CleanTeeth API";
        document.Info.Version = "v1";
        document.Info.Description = "基于 Minimal API 和垂直切片架构的牙科诊所管理系统";
        return Task.CompletedTask;
    });
});

var app = builder.Build();

// ==========================================
// 2. 中间件管道配置阶段 (Middleware Pipeline)
// ==========================================

// 架构规范：API 文档和测试界面仅应在开发环境中暴露，避免生产环境泄露内部契约
if (app.Environment.IsDevelopment())
{
    // 映射 OpenAPI 规范端点 (默认路径: /openapi/v1.json)
    app.MapOpenApi();

    // 映射 Scalar UI (默认路径: /scalar/v1)
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("CleanTeeth API Reference");
        options.WithTheme(ScalarTheme.Default); // 可根据项目 VI 调整主题
    });
}
app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseRouting();

// ==========================================
// 3. 端点映射阶段 (Endpoint Mapping)
// ==========================================

// 核心：通过反射自动扫描并注册所有实现了 IEndpoint 接口的垂直切片端点
app.MapEndpoints();

app.Run();