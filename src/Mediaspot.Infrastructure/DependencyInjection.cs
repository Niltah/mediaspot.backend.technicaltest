using FluentValidation;
using Mediaspot.Application.Assets.Commands.Create;
using Mediaspot.Application.Common;
using Mediaspot.Application.Titles.Commands.Create;
using Mediaspot.Application.Titles.Commands.Update;
using Mediaspot.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Mediaspot.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string databaseName)
    {
        // Persistence
        services.AddDbContext<MediaspotDbContext>(o => o.UseInMemoryDatabase(databaseName));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IAssetRepository, AssetRepository>();
        services.AddScoped<ITranscodeJobRepository, TranscodeJobRepository>();
        services.AddScoped<ITitleRepository, TitleRepository>();

        // Validators
        services.AddScoped<IValidator<CreateAssetCommand>, CreateAssetValidator>();
        services.AddScoped<IValidator<CreateTitleCommand>, CreateTitleValidator>();
        services.AddScoped<IValidator<UpdateTitleCommand>, UpdateTitleValidator>();

        // MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateAssetCommand).Assembly));

        return services;
    }
}