using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using PaymentTransaction.Data;
using PaymentTransaction.Mappings;
using PaymentTransaction.Repositories;
using System.Reflection;
using PaymentTransaction.Attributes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Register the Swagger generator with annotations support
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations(); // Enable processing of Swagger annotations

    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Your API Title",
        Description = "Your API Description"
    });

    // Include XML comments if you have them
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }

    // Register the custom schema filter
    options.SchemaFilter<SwaggerSchemaExampleFilter>();

    // ✅ Register the operation filter that adds Idempotency-Key to Swagger UI
    options.OperationFilter<AddIdempotencyKeyHeaderParameter>();
});

// Register IdempotencyFilter
builder.Services.AddScoped<IdempotencyFilter>();

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