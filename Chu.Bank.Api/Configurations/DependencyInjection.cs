using Chu.Bank.Api.Domain.Repositories;
using Chu.Bank.Api.Domain.Services;
using Chu.Bank.Api.Persistence.Contexts;
using Chu.Bank.Api.Persistence.Repositories;
using Chu.Bank.Api.Persistence.Services;
using Chu.Bank.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace Chu.Bank.Api.Configurations;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureWebApi(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        return services;
    }

    public static IServiceCollection ConfigureBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ITransferService, TransferService>();
        return services;
    }

    public static IServiceCollection ConfigureDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options => 
            options.UseSqlServer(configuration.GetConnectionString("ChuBankConnection")));

        services.AddMemoryCache();

        services.AddHttpClient<IBusinessDayService, BusinessDayService>();

        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ITransferRepository, TransferRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
