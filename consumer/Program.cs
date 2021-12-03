using Microsoft.EntityFrameworkCore;
using RabbitConsumer;
using RabbitConsumer.Storage;

IHostBuilder builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((hostContext, services) =>
    {
        services.AddDbContext<Context>(options =>
            options.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultConnection"))
        );
        services.AddHostedService<Consumer>();
    });

var app = builder.Build();

await app.RunAsync();
