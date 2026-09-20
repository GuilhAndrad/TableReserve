using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TableReserve.Domain.Repositories;
using TableReserve.Domain.Repositories.User;
using TableReserve.Domain.Security.PasswordHashing;
using TableReserve.Domain.Security.Tokens;
using TableReserve.Infrastructure.DataAccess;
using TableReserve.Infrastructure.DataAccess.Repositories;
using TableReserve.Infrastructure.Security.PasswordHashing;
using TableReserve.Infrastructure.Security.Tokens;

namespace TableReserve.Infrastructure;

public static class DependencyInjectionExtension
{
    extension(IServiceCollection services)
    {
        public void AddInfrastructure(IConfiguration configuration)
        {
            services.AddRepositories();

            services.AddSecurity(configuration);

            services.AddScoped<ILoggedUser, LoggedUser>();

            services.AddDbContext<TableReserveDbContext>(config =>
            {
                string connectionString = configuration.GetConnectionString("DbConnection")!;

                config.UseMySQL(connectionString);
            });

            services.AddFluentMigratorCore().ConfigureRunner(config =>
            {
                config
                .AddMySql5()
                .WithGlobalConnectionString(_ =>
                {
                    string connectionString = configuration.GetConnectionString("DbConnection")!;

                    return connectionString;
                })
                .ScanIn(Assembly.Load("TableReserve.Infrastructure"))
                .For.All();
            });
        }

        private void AddRepositories()
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserWriter, UserRepository>();
            services.AddScoped<IUserReader, UserRepository>();
        }

        private void AddSecurity(IConfiguration configuration)
        {
            services.AddScoped<IPasswordHasher, Argon2PasswordHasher>();
            services.AddScoped<IAccessTokenGenerator>(provider =>
            {
                uint tokenExpirationInMinutes = configuration.GetValue<uint>("Security:TokenExpirationInMinutes");
                string secretKey = configuration.GetValue<string>("Security:SecretKey")!;
                return new JwtTokenHandler(tokenExpirationInMinutes, secretKey);
            });
        }
    }
}