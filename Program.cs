using Microsoft.AspNetCore.RateLimiting;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using ViteMontevideo_API.Configuration;
using ViteMontevideo_API.Presentation.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(opt =>
        {
            opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

builder.Services.AddRateLimiter(opt =>
{
    opt.AddFixedWindowLimiter(nameof(RateLimitPolicy.HighFrequencyPolicy), opt =>
    {
        opt.PermitLimit = 20;
        opt.Window = TimeSpan.FromMinutes(2);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });

    opt.AddFixedWindowLimiter(nameof(RateLimitPolicy.LowFrequencyPolicy), opt =>
    {
        opt.PermitLimit = 10;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });

    opt.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

builder.Services.AddCustomServices(builder.Configuration);
builder.Services.AddSwaggerServices();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{

//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseCors("ReglasCors");

app.UseRateLimiter();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
