using FluentMigrator;

namespace TableReserve.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.Add_Tables_Table, "Creating Tables table")]
public class Version0002 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Tables")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("Name").AsString(250).NotNullable()
            .WithColumn("Capacity").AsInt32().NotNullable()
            .WithColumn("Status").AsString(50).NotNullable();
    }
}