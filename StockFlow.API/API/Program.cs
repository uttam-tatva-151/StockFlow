using StockFlow.API.Middlewares;
using StockFlow.Repository.Extensions;
using StockFlow.Service.Extensions;
using Microsoft.AspNetCore.Mvc;
using StockFlow.API.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddConfigurationBindings(builder.Configuration);
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

builder.Services.RegisterDBContext(builder.Configuration);

builder.Services.RegisterServices();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
