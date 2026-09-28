using FluentMigrator;

namespace TableReserve.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.Add_Reservations_Table, "Creating Reservations table")]
public class Version0003 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Reservations")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("UserId").AsGuid().NotNullable().ForeignKey("Users", "Id")
            .WithColumn("TableId").AsGuid().NotNullable().ForeignKey("Tables", "Id")
            .WithColumn("ReservationDate").AsDateTime().NotNullable()
            .WithColumn("DurationMinutes").AsInt32().NotNullable()
            .WithColumn("GuestsCount").AsInt32().NotNullable()
            .WithColumn("Status").AsString(50).NotNullable();
    }
}