using WorkerHttp;
using Md5_Selection.HashWorkers.InterfaceHashWorker;
using Md5_Selection.HashWorkers;
using WorkerHttp.TimeoutPerformers;
using WorkerHttp.Requests;
using System.Reflection;
using MassTransit;
using WorkerHttpV2.Consumers;
using WorkerHttpV2.Requests;
using MessagesBetweenManagerAndWorker;

var builder = WebApplication.CreateBuilder(args);
var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).AddEnvironmentVariables()
    .Build();
var executorTimeoutforHashWorkerConfig = config.GetSection("ExecutorTimeoutforHashWorkerConfig").Get<ExecutorTimeoutforHashWorkerConfig>();
builder.Services.AddSingleton(executorTimeoutforHashWorkerConfig);
builder.Services.AddSingleton<IExecutorTimeOut>(sp => new ExecutorTimeoutforHashWorker(sp.GetRequiredService<ExecutorTimeoutforHashWorkerConfig>()));
//builder.Services.AddSingleton<IRequest>(sp => new RequestToManager(sp.GetRequiredService<RequestToManagerConfig>()));
builder.Services.AddSingleton<IRequest, RequestToManagerForConsumer>();
builder.Services.AddSingleton<IHashWorker, Md5HashWorker>();
builder.Services.Configure<AlphabetSetting>(config.GetSection("AlphabetSetting"));
builder.Services.Configure<RequestToManagerConfig>(config.GetSection(nameof(RequestToManagerConfig)));
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    x.SetInMemorySagaRepositoryProvider();
    var entryAssembly = Assembly.GetEntryAssembly();
    x.AddConsumers(entryAssembly);
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq", "/", h =>
        {
            h.Username("quest");
            h.Password("quest");
        });
        cfg.ReceiveEndpoint("Worker_queue", ep =>
        {
            ep.Consumer<Consumer>(context);
            ep.UseMessageRetry(r => r.Interval(5, TimeSpan.FromSeconds(3)));
            ep.ConfigureConsumer<Consumer>(context, c =>
            {
                c.Message<HashCodeMessage>(m => m.UseConcurrencyLimit(5)); // ”казываете количество потоков
            });

        });
        

    });
   

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
