using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NLPDemo.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblproperty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SurveyNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubDivision = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Village = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Taluk = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FatherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AreaAcre = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AreaGunta = table.Column<int>(type: "int", nullable: false),
                    LandType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LastPaidYear = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KhataNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MutationStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Encumbrance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblproperty", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblvillageinfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VillageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalPopulation = table.Column<int>(type: "int", nullable: false),
                    Male = table.Column<int>(type: "int", nullable: false),
                    Female = table.Column<int>(type: "int", nullable: false),
                    Households = table.Column<int>(type: "int", nullable: false),
                    LiteracyRate = table.Column<double>(type: "float", nullable: false),
                    SC = table.Column<int>(type: "int", nullable: false),
                    ST = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblvillageinfo", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblproperty");

            migrationBuilder.DropTable(
                name: "tblvillageinfo");
        }
    }
}
