using Microsoft.Extensions.DependencyInjection;
using Ucelo.Application.Services.Implementations;
using Ucelo.Application.Services.Interfaces;

namespace Ucelo.Application.DependencyInjection;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IIndividualService, IndividualService>();
        services.AddScoped<ICompanyService, CompanyService>();

        return services;
    }
}