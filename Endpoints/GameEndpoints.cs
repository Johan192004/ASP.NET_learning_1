using System;
using GameStore.Api.DTOs;

namespace GameStore.Api.Endpoints;

public static class GameEndpoints
{
    private static List<GameDto> games = new List<GameDto>
    {
        new GameDto(1, "The Legend of Zelda: Breath of the Wild", "Action-adventure", 59.99m, new DateOnly(2017, 3, 3)),
        new GameDto(2, "Super Mario Odyssey", "Platformer", 59.99m, new DateOnly(2017, 10, 27)),
        new GameDto(3, "Red Dead Redemption 2", "Action-adventure", 59.99m, new DateOnly(2018, 10, 26)),
        new GameDto(4, "The Witcher 3: Wild Hunt", "Action RPG", 39.99m, new DateOnly(2015, 5, 19)),
        new GameDto(5, "Minecraft", "Sandbox", 26.95m, new DateOnly(2011, 11, 18))
    };

    const string GetGameEndpointName = "GetGameById";

    public static void MapGameEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games");

        //GET /games
        group.MapGet("/", () => games);


        //GET /games/{id}
        group.MapGet("/{id}", (int id) =>
        {
            var game = games.Find(game => game.Id == id);
            return game is not null ? Results.Ok(game) : Results.NotFound();
        })
            .WithName(GetGameEndpointName);

        //POST /games

        group.MapPost("/", (CreateGameDto newGame) =>
        {
            // if(string.IsNullOrWhiteSpace(newGame.Name))
            // {
            //     return Results.BadRequest("Name is required.");
            // }

            GameDto game = new(
                games.Count + 1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseDate
            );

            games.Add(game);

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
        });


        //PUT /games/{id}
        group.MapPut("/{id}", (int id, UpdateGameDto updateGame) =>
        {
            int index = games.FindIndex(game => game.Id == id);

            if (index != -1)
            {
                GameDto game = new(
                    id,
                    updateGame.Name,
                    updateGame.Genre,
                    updateGame.Price,
                    updateGame.ReleaseDate
                );

                games[index] = game;

                return Results.NoContent();
            }
            else
            {
                return Results.NotFound();
            }

        });


        //DELETE /games/{id}
        group.MapDelete("/{id}", (int id) =>
        {
            int index = games.FindIndex(game => game.Id == id);

            if (index != -1)
            {
                games.RemoveAt(index);
                return Results.NoContent();
            }
            else
            {
                return Results.NotFound();
            }
        });
    }

}
