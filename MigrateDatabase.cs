using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mottu.Infrastructure;

class Program
{
    static void Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddDbContext<MottuDbContext>(options =>
                options.UseNpgsql("Host=postgres;Port=5432;Username=postgres;Password=postgres;Database=mottu"))
            .BuildServiceProvider();

        using (var scope = serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<MottuDbContext>();
            dbContext.Database.Migrate();
            Console.WriteLine("Database migration applied.");
        }
    }
}
