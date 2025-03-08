using Serilog;
using SerilogEx;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddLogging(loBuilder =>
{
    loBuilder.ClearProviders();
    loBuilder.AddConsole();
    loBuilder.AddSerilog(CreateSeriloLogger(), dispose: true);
});

var host = builder.Build();
host.Run();

static Serilog.ILogger CreateSeriloLogger()
{
    var logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .WriteTo.Logger(lc =>
        {
            lc.Filter.ByIncludingOnly(p => p.Level == Serilog.Events.LogEventLevel.Debug ||
            p.Level == Serilog.Events.LogEventLevel.Fatal)
            .WriteTo.File("logdata.txt", flushToDiskInterval: new TimeSpan(0, 0, 0, 10));
        }).CreateLogger();

    return logger;
}

static Serilog.ILogger CreateSeriloLoggerMongoDb(HostApplicationBuilder builder)
{
    var logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration);
        //.MinimumLevel.Debug()
        //.WriteTo.Logger(lc =>
        //{
        //    lc.Filter.ByIncludingOnly(p => p.Level == Serilog.Events.LogEventLevel.Debug ||
        //    p.Level == Serilog.Events.LogEventLevel.Fatal)
        //    .WriteTo.MongoDB("mongoDb://localhost:27017/demo", collectionName: "logs", batchPostingLimit: 1);
        //}).CreateLogger();

    return logger;
}
