using controle_de_estacionamento.Repositories;
using controle_de_estacionamento.Repositories.Interfaces;
using controle_de_estacionamento.Services;
using controle_de_estacionamento.Services.IServices;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options => options.AddPolicy("MyPolicy", policy =>
{
    policy.WithOrigins("http://localhost:3000")
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials();
}));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IParkingService, ParkingService>();
builder.Services.AddTransient<IPriceService, PriceService>();

builder.Services.AddTransient<IParkingRepository, ParkingRepository>();
builder.Services.AddTransient<IPriceRepository, PriceRepository>();

#if TEST
builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseTest"),
    sqlServerOptionsAction: sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    }));
#else
builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database"),
    sqlServerOptionsAction: sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    }));
#endif

var app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    Context context = scope.ServiceProvider.GetRequiredService<Context>();
#if !TEST
    context.Database.EnsureCreated();
#elif TEST
    if (!context.Database.EnsureCreated())
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }
#endif
}

app.UseCors("MyPolicy");

using (IServiceScope scope = app.Services.CreateScope())
{
    Context context = scope.ServiceProvider.GetRequiredService<Context>();

    context.Database.EnsureCreated();
}

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
