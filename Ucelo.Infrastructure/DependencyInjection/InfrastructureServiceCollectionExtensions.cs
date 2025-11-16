using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System.Security.Cryptography;
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
        AddSecurity(services, configuration);
        AddAuthentication(services, configuration);

        return services;
    }

    private static void ConfigureNpgsqlEnumMapping()
    {
        lock (_lock)
        {
            if (_enumsMapped) return;

            // Mapeamento existente
            NpgsqlConnection.GlobalTypeMapper.MapEnum<UserType>(
                "type_user",
                new NpgsqlNameTranslator()
            );

            // Adicionar este novo mapeamento
            NpgsqlConnection.GlobalTypeMapper.MapEnum<CalculationType>(
                "calculation_type",
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
        services.AddScoped<ILoginAttemptRepository, LoginAttemptRepository>();
        services.AddScoped<IBucketRepository, BucketRepository>();
        services.AddScoped<IMaterialRepository, MaterialRepository>();
    }

    private static void AddSecurity(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IJwtService, JwtService>();
        services.AddScoped<IRateLimitService, RateLimitService>();

        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
        services.Configure<RateLimitSettings>(configuration.GetSection("RateLimiting"));
        services.AddScoped<ICalculationRepository, CalculationRepository>();
    }

    private static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>();

        if (jwtSettings == null)
            throw new InvalidOperationException("Configuração JWT não encontrada");

        var publicKeyBytes = Convert.FromBase64String(jwtSettings.PublicKeyBase64);
        var rsa = RSA.Create();

        rsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new RsaSecurityKey(rsa),
                    ClockSkew = TimeSpan.Zero
                };
            });

        services.AddAuthorization();
    }

    private class NpgsqlNameTranslator : INpgsqlNameTranslator
    {
        public string TranslateTypeName(string clrName) => clrName switch
        {
            nameof(UserType.Common) => "Comum",
            nameof(UserType.Seller) => "Vendedor",
            nameof(UserType.Master) => "Master",
            // Adicionar as traduções do CalculationType
            nameof(CalculationType.Power) => "potencia",
            nameof(CalculationType.Tension) => "tensao",
            nameof(CalculationType.Comparison) => "comparacao",
            _ => clrName
        };

        public string TranslateMemberName(string clrName) => TranslateTypeName(clrName);
    }
}