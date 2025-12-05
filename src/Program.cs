// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using eventhub_custom_metrics_emitter;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        if (context.HostingEnvironment.IsDevelopment())
        {
            config.AddUserSecrets<Program>();
        }
    })
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;

        if (!string.IsNullOrEmpty(configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]))
        {
            services.AddApplicationInsightsTelemetryWorkerService();
        }
        services.AddHostedService<Worker>();
        services.AddLogging(opt =>
        {
            opt.AddSimpleConsole(config => config.TimestampFormat = "[HH:mm:ss]");
        });

    })
    .Build();

await host.RunAsync();
