// Orleans服务器
using HelloWorld;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

/*
using var host = new HostBuilder()
    .UseOrleans(builder =>
    {
        builder.UseLocalhostClustering();
    })
    .Build();

await host.StartAsync();

// Get the grain factory
var grainFactory = host.Services.GetRequiredService<IGrainFactory>();

// Get a reference to the HelloGrain grain with the key "friend".
var friend = grainFactory.GetGrain<IHelloGrain>("friend");

// Call the grain and print the result to the console
var result = await friend.SayHello("Good morning!");
Console.WriteLine("\n\n{0}\n\n", result);

Console.WriteLine("Orleans is running.\nPress Enter to terminate...");
Console.ReadLine();
Console.WriteLine("Orleans is stopping...");

await host.StopAsync();
*/

///*
Console.Title = "Orleans Server";

//var host = new HostBuilder();
//await host.RunConsoleAsync();
//await host.StartAsync();
//await Host.CreateDefaultBuilder(args)
await new HostBuilder()
    .UseOrleans(builder => builder
            .UseLocalhostClustering()
            .UseDashboard())
    .ConfigureLogging(logging => logging.AddConsole()) //使控制台显示日志
    .RunConsoleAsync();
//*/