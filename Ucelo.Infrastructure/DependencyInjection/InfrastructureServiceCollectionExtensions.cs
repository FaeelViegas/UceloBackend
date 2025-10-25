using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Ucelo.Domain.Enums;
using Ucelo.Domain.Interfaces;
using Ucelo.Infrastructure.Data;
using Ucelo.Infrastructure.Repositories;
using Ucelo.Infrastructure.Security;

namespace Ucelo.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    private static bool _enumsMapped = false;
    private static readonly object _lock = new();

    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ConfigureNpgsqlEnumMapping();
        
        AddDatabase(services, configuration);
        AddRepositories(services);
        AddSecurity(services);

        return services;
    }

    private static void ConfigureNpgsqlEnumMapping()
    {
        lock (_lock)
        {
            if (_enumsMapped) return;

            NpgsqlConnection.GlobalTypeMapper.MapEnum<UserType>(
                "type_user",
                new NpgsqlNameTranslator()
            );

            _enumsMapped = true;
        }
    }

    private static void AddDatabase(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<UceloDbContext>(options =>
        {
            options.UseNpgsql(connectionString);    
            options.EnableSensitiveDataLogging(false);
            options.EnableDetailedErrors(false);
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IIndividualRepository, IndividualRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
    }

    private static void AddSecurity(IServiceCollection services)
    {
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
    }

    private class NpgsqlNameTranslator : INpgsqlNameTranslator
    {
        public string TranslateTypeName(string clrName) => clrName switch
        {
            nameof(UserType.Common) => "Comum",
            nameof(UserType.Seller) => "Vendedor",
            nameof(UserType.Master) => "Master",
            _ => clrName
        };

        public string TranslateMemberName(string clrName) => TranslateTypeName(clrName);
    }
}