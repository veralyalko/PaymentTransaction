using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using PaymentTransaction.Data;
using PaymentTransaction.Mappings;
using PaymentTransaction.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Db Class DI
builder.Services.AddDbContext<PaymentTransactionDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("PaymentTransactionConnectionString")));

// Repository services
builder.Services.AddScoped<IProviderRepository, SQLProviderRepository>();
builder.Services.AddScoped<ICurrencyRepository, SQLCurrencyRepository>();
builder.Services.AddScoped<IStatusRepository, SQLStatusRepository>();
builder.Services.AddScoped<IPaymentMethodRepository, SQLPaymentMethodRepository>();
builder.Services.AddScoped<ITransactionRepository, SQLTransactionRepository>();
builder.Services.AddScoped<IStatusRepository, SQLStatusRepository>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

var app = builder.Build();

// Dev Ex Page
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // ← put it right here
}
else
{
    app.UseExceptionHandler("/error");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();