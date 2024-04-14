using ManagerHttp;
using ManagerHttp.Requests;
using ManagerHttpV2.Consumers;
using ManagerHttpV2.Requests;
using MassTransit;
using System.Reflection;
using MongoDB.Driver;
using ManagerHttpV2.DBConfig;
using MessagesBetweenManagerAndWorker;

var builder = WebApplication.CreateBuilder(args);
var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).AddEnvironmentVariables()
    .Build();

var requestToWorkerConfig = config.GetSection("RequestToWorkerConfig").Get<RequestToWorkerConfig>();
builder.Services.AddSingleton(requestToWorkerConfig);
// Add services to the container.
builder.Services.AddSingleton<ProjectionTaskRepository>();
//builder.Services.AddSingleton<IRequest>(sp => new RequestToWorker(sp.GetRequiredService<RequestToWorkerConfig>()));
builder.Services.AddSingleton<IRequest, RequestToWorkerForConsumer>();
builder.Services.Configure<RequestToWorkerConfig>(config.GetSection("RequestToWorkerConfig"));
builder.Services.Configure<DBConfig>(config.GetSection("DBConfig"));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//builder.Services.AddHostedService

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
        cfg.ReceiveEndpoint("Manager_queue", ep =>
        {
            ep.Consumer<PatchTaskConsumer>(context);
            ep.UseMessageRetry(r => r.Interval(5, TimeSpan.FromSeconds(1)));
            ep.ConfigureConsumer<PatchTaskConsumer>(context, c =>
            {
                c.Message<MessageForDecryptedWord>(m => m.UseConcurrencyLimit(5)); // ”казываете количество потоков
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.Run();
