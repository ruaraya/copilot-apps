using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Minimizer.API.Services;
using Minimizer.Data.DbContexts;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddMediatR(typeof(Minimizer.Services.MediatREntrypoint).Assembly); // Scans the current assembly

builder.Services.AddScoped<AuthService>();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    //cfg.RegisterServicesFromAssembly(typeof(Minimizer.Services.Users).Assembly);
});


builder.Services.AddDbContext<MinimizerDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 21))));

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
