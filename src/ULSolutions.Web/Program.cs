using AspNetCoreRateLimit;
using ULSolutions.Core.Interfaces;
using ULSolutions.Core.Mapper;
using ULSolutions.Core.Services;
using WatchDog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<IExpressionEvaluatorFetcher, ExpressionEvaluatorFetcher>();
builder.Services.AddScoped<IExpressionEvaluatorService, ExpressionEvaluatorService>();

if (Convert.ToBoolean(builder.Configuration["EnableCaching"]))
{
    builder.Services.Decorate<IExpressionEvaluatorService, ExpressionEvaluatorServiceDecorator>();
}

builder.Services.AddWatchDogServices();
builder.Logging.AddWatchDogLogger();



builder.Services.AddDistributedMemoryCache();
builder.Services.AddMemoryCache();
//// Services add for rate limiting
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
builder.Services.AddInMemoryRateLimiting();



var app = builder.Build();

app.MapHealthChecks("/health");


//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseIpRateLimiting();

app.MapControllers();

// Injecting MiddleWare.
app.UseWatchDogExceptionLogger();
app.UseWatchDog(opt =>
{
    opt.WatchPageUsername = "admin"; //Environment.GetEnvironmentVariable("WATCHDOG_USERNAME"); 
    opt.WatchPagePassword = "admin"; //Environment.GetEnvironmentVariable("WATCHDOG_PASSWORD"); 
});

app.Run();