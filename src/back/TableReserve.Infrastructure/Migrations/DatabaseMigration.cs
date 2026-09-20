using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace TableReserve.Infrastructure.Migrations;

public class DatabaseMigration
{
    public static void RunnerMigrations(IServiceProvider serviceProvider)
    {
        IMigrationRunner runner = serviceProvider.GetRequiredService<IMigrationRunner>();

        runner.ListMigrations();

        runner.MigrateUp();
    }
}