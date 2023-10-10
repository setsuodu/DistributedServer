// Orleans客户端
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Client;

Console.Title = "Orleans Client";

//await new HostBuilder()
//    .UseOrleansClient(builder => builder
//        .UseLocalhostClustering())
//    .ConfigureLogging(logging => logging.AddConsole())
//    .RunConsoleAsync();

// ①启动Orleans客户端
using var host = new HostBuilder()
    .UseOrleansClient(b => b
        .UseLocalhostClustering())
    .ConfigureLogging(logging => logging.AddConsole())
    .Build();

IClusterClient client = host.Services.GetRequiredService<IClusterClient>();
await host.StartAsync();

// ②启动Unity专用服务器
var unityServer = new UnityServer();
unityServer.Start();
