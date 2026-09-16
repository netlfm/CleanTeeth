using CleanTeeth.API.Endpoints;

namespace CleanTeeth.API.Infrastructure;

public static class EndpointExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        // 扫描当前程序集中所有实现了 IEndpoint 接口的非抽象类
        var endpointTypes = typeof(EndpointExtensions).Assembly
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && typeof(IEndpoint).IsAssignableFrom(t));

        foreach (var endpointType in endpointTypes)
        {
            // 实例化端点类并执行路由映射
            var endpoint = (IEndpoint)Activator.CreateInstance(endpointType, nonPublic: true)!;
            endpoint.MapEndpoint(app);
        }

        return app;
    }
}
