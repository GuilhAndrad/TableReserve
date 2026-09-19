using FluentMigrator;

namespace TableReserve.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.Add_Users_Table, "Creating Users table")]
public class Version0001 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Users")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("Name").AsString(250).NotNullable()
            .WithColumn("Email").AsString(250).NotNullable().Unique()
            .WithColumn("Password").AsString(255).NotNullable()
            .WithColumn("Role").AsString(50).NotNullable();
    }
}