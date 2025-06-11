using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeadManagementApi.Migrations
{
    public partial class AddLeadStatusAndContactInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactEmail",
                table: "Leads",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactFullName",
                table: "Leads",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactPhoneNumber",
                table: "Leads",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Leads",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactEmail",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "ContactFullName",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "ContactPhoneNumber",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Leads");
        }
    }
}
