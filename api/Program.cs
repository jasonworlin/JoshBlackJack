using Microsoft.EntityFrameworkCore;
using api.Services;

internal class Program
{
    private static void Main(string[] args)
    {
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

        builder.Services.AddScoped<IEmailVerificationService, EmailVerificationService>();
        builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
        builder.Services.AddScoped<IGameEngine, GameEngine>();              

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
    }
}