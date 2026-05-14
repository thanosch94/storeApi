using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StoreApi.Data;
using StoreApi.Data.Models;
using StoreApi.Filters;
using StoreApi.Interfaces;
using StoreApi.Processors;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ActionLoggingFilter>();
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var provider = builder.Configuration.GetSection("Provider").Value;
    if (provider == "MsSQL")
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
        options.UseSqlServer(x => x.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));

        AppBase.ConnectionString = builder.Configuration.GetConnectionString("DbConnection");

    }

});

builder.Services.AddScoped<IConsentProcessor, ConsentProcessor>();

builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var origins = builder.Configuration["AllowedOrigins"].Split(";");

if (origins.Length > 0 && !string.IsNullOrEmpty(origins[0]))
{
    app.UseCors(x => x.AllowAnyMethod().AllowAnyHeader().WithOrigins(origins).AllowCredentials());
}
else
{
    app.UseCors(x => x.SetIsOriginAllowed(origin => true).AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
