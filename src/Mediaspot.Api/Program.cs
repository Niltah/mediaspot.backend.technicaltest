using Mediaspot.Api.DTOs;
using Mediaspot.Application.Assets.Commands.Archive;
using Mediaspot.Application.Assets.Commands.Create;
using Mediaspot.Application.Assets.Commands.RegisterMediaFile;
using Mediaspot.Application.Assets.Commands.UpdateMetadata;
using Mediaspot.Application.Assets.Queries.GetById;
using Mediaspot.Application.Titles.Commands.Create;
using Mediaspot.Application.Titles.Commands.Update;
using Mediaspot.Application.Titles.Queries.GetAll;
using Mediaspot.Application.Titles.Queries.GetById;
using Mediaspot.Infrastructure;
using Mediaspot.Infrastructure.Persistence;
using MediatR;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddInfrastructure("Mediaspot.Backend.TechnicalTest");

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<MediaspotDbContext>();
            db.Database.EnsureCreated();
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        MapAssets(app);
        MapTitles(app);

        app.Run();
    }

    private static void MapAssets(WebApplication app)
    {

        app.MapGet("/assets/{id:guid}", async (Guid id, ISender sender) =>
        {
            var asset = await sender.Send(new GetAssetByIdQuery(id));
            return Results.Ok(asset);
        })
            .WithName("GetAssetById")
            .WithOpenApi();

        app.MapPost("/assets", async (CreateAssetCommand cmd, ISender sender) => Results.Created("/assets", new { id = await sender.Send(cmd) }))
            .WithName("PostCreateAsset")
            .WithOpenApi();

        app.MapPost("/assets/{id:guid}/files", async (Guid id, string path, double durationSeconds, ISender sender)
                => Results.Ok(new { mediaFileId = await sender.Send(new RegisterMediaFileCommand(id, path, durationSeconds)) }))
            .WithName("PostRegisterMediaFile")
            .WithOpenApi();

        app.MapPut("/assets/{id:guid}/metadata", async (Guid id, UpdateMetadataDto dto, ISender sender) =>
        {
            await sender.Send(new UpdateMetadataCommand(id, dto.Title, dto.Description, dto.Language));
            return Results.NoContent();
        })
            .WithName("PutUpdateMetadata")
            .WithOpenApi();

        app.MapPost("/assets/{id:guid}/archive", async (Guid id, ISender sender) =>
        {
            await sender.Send(new ArchiveAssetCommand(id));
            return Results.NoContent();
        })
            .WithName("PostArchiveAsset")
            .WithOpenApi();
    }
    private static void MapTitles(WebApplication app)
    {

        app.MapGet("/titles/{id:guid}", async (Guid id, ISender sender) =>
        {
            var title = await sender.Send(new GetTitleByIdQuery(id));
            return Results.Ok(title);
        })
            .WithName("GetTitleById")
            .WithOpenApi();

        app.MapGet("/titles", async (ISender sender) =>
        {
            var titles = await sender.Send(new GetAllTitlesQuery());
            return Results.Ok(titles);
        })
            .WithName("GetAllTitles")
            .WithOpenApi();

        app.MapPost("/titles", async (CreateTitleCommand cmd, ISender sender)
            => Results.Created("/titles", new { id = await sender.Send(cmd) }))
            .WithName("PostCreateTitle")
            .WithOpenApi();

        app.MapPut("/titles/{id:guid}", async (Guid id, UpdateTitleDto dto, ISender sender) =>
        {
            await sender.Send(new UpdateTitleCommand(id, dto.Name, dto.Description, dto.ReleaseDate, dto.Type));
            return Results.NoContent();
        })
            .WithName("PutUpdateTitle")
            .WithOpenApi();
    }
}