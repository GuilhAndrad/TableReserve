using FluentMigrator;

namespace TableReserve.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.Add_RefreshTokens_Table, "Creating RefreshTokens table")]
public class Version0004 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("RefreshTokens")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("Value").AsString(255).NotNullable()
            .WithColumn("CreatedAt").AsDateTime().NotNullable()
            .WithColumn("UserId").AsGuid().NotNullable()
                .ForeignKey("FK_RefreshTokens_Users_UserId", "Users", "Id");
    }
}