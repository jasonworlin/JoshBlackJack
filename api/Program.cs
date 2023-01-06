using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<GameDb>(options =>
options.UseSqlite(builder.Configuration.GetConnectionString("BlackJackSQLite")));

builder.Services.AddDbContext<UserDb>(options =>
options.UseSqlite(builder.Configuration.GetConnectionString("BlackJackSQLite")));

builder.Services.AddSwaggerGen();

var app = builder.Build();
//app.MapGet("/jason", () => new { FirstName = "Gérald", LastName = "Barré" });

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
