using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeagueChampsDb.Migrations
{
    public partial class Abilities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Abilities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPassive = table.Column<bool>(type: "bit", nullable: false),
                    ChampionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Abilities_Champions_ChampionId",
                        column: x => x.ChampionId,
                        principalTable: "Champions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Abilities_ChampionId",
                table: "Abilities",
                column: "ChampionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Abilities");
        }
    }
}
