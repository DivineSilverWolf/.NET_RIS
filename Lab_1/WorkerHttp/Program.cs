using WorkerHttp;
using Md5_Selection.HashWorkers.InterfaceHashWorker;
using Md5_Selection.HashWorkers;
using WorkerHttp.TimeoutPerformers;
using WorkerHttp.Новая_папка;
using WorkerHttp.Requests;

var builder = WebApplication.CreateBuilder(args);
var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).AddEnvironmentVariables()
    .Build();
var executorTimeoutforHashWorkerConfig = config.GetSection("ExecutorTimeoutforHashWorkerConfig").Get<ExecutorTimeoutforHashWorkerConfig>();
var requestManagerConfig = config.GetSection("RequestToManagerConfig").Get<RequestToManagerConfig>();
builder.Services.AddSingleton(executorTimeoutforHashWorkerConfig);
builder.Services.AddSingleton(requestManagerConfig);
builder.Services.AddSingleton<IExecutorTimeOut>(sp => new ExecutorTimeoutforHashWorker(sp.GetRequiredService<ExecutorTimeoutforHashWorkerConfig>()));
builder.Services.AddSingleton<IRequest>(sp => new RequestToManager(sp.GetRequiredService<RequestToManagerConfig>()));
builder.Services.AddSingleton<IHashWorker, Md5HashWorker>();
builder.Services.Configure<AlphabetSetting>(config.GetSection("AlphabetSetting"));
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
