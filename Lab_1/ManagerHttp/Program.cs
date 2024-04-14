using ManagerHttp;
using ManagerHttp.Requests;

var builder = WebApplication.CreateBuilder(args);
var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).AddEnvironmentVariables()
    .Build();
var requestToWorkerConfig = config.GetSection("RequestToWorkerConfig").Get<RequestToWorkerConfig>();
builder.Services.AddSingleton(requestToWorkerConfig);
// Add services to the container.
builder.Services.AddSingleton<SingletonDictonaryIdToWord>();
builder.Services.AddSingleton<IRequest>(sp => new RequestToWorker(sp.GetRequiredService<RequestToWorkerConfig>()));
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
