using DatabaseApi.Model;
using DatabaseApi.Model.Context;
using DatabaseApi.Model.Entities;
using DatabaseApi.Model.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var cb = new ConfigurationBuilder();
cb.AddJsonFile("./appsettings.json");
var config = cb.Build();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(config.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddScoped<IRepository<ApiUser>, ApiUsersRepository>();
builder.Services.AddScoped<IRepository<ApiUserRight>, ApiUserRightsRepository>();
builder.Services.AddScoped<IRepository<Article>, ArticleRepository>();
builder.Services.AddScoped<IRepository<Keyword>, KeywordsRepository>();
builder.Services.AddScoped<ApiUserService>(); 
//builder.Services.AddScoped<ApplicationContext>();




var app = builder.Build();
app.UseCors(builder =>
{
    builder.AllowAnyOrigin();
    builder.AllowAnyHeader();
});
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