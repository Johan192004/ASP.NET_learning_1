using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public static class DataExtensions
{

    public static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();

        dbContext.Database.Migrate();
    }

    public static void AddGameStoreDb(this WebApplicationBuilder builder)
    {

        //No sabe de donde viene la informacion
        var conString = builder.Configuration.GetConnectionString("GameStore") ?? "Data Source=GameStore.db";
        builder.Services.AddSqlite<GameStoreContext>(conString, optionsAction: options => options.UseSeeding((context, _) =>
        {
            if (!context.Set<Genre>().Any())
            {
                context.Set<Genre>().AddRange(
                    new Genre { Name = "Action" },
                    new Genre { Name = "Adventure" },
                    new Genre { Name = "RPG" },
                    new Genre { Name = "Strategy" },
                    new Genre { Name = "Sports" }
                );

                context.SaveChanges();
            }
        }));
    }

}
