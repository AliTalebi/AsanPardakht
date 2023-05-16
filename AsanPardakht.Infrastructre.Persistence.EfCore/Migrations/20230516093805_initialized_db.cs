using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsanPardakht.Infrastructre.Persistence.EfCore.Migrations
{
    public partial class initialized_db : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "BaseInformation");

            migrationBuilder.EnsureSchema(
                name: "Outbox");

            migrationBuilder.EnsureSchema(
                name: "People");

            migrationBuilder.CreateSequence<int>(
                name: "Cities",
                minValue: 1L,
                maxValue: 2147483647L);

            migrationBuilder.CreateSequence<int>(
                name: "People",
                minValue: 1L,
                maxValue: 2147483647L);

            migrationBuilder.CreateSequence<int>(
                name: "Provinces",
                minValue: 1L,
                maxValue: 2147483647L);

            migrationBuilder.CreateTable(
                name: "OutBoxEvents",
                schema: "Outbox",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IssuedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValue: "ali.talebi"),
                    EventType = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    IssuedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    AggregateId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    AggregateType = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    Read = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ReadAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutBoxEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "People",
                schema: "People",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NationalCode = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Provinces",
                schema: "BaseInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provinces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                schema: "BaseInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProvinceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cities_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalSchema: "BaseInformation",
                        principalTable: "Provinces",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PeopleAddresses",
                schema: "People",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Detail = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    ProvinceId = table.Column<int>(type: "int", nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeopleAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PeopleAddresses_Cities_CityId",
                        column: x => x.CityId,
                        principalSchema: "BaseInformation",
                        principalTable: "Cities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PeopleAddresses_People_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "People",
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PeopleAddresses_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalSchema: "BaseInformation",
                        principalTable: "Provinces",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cities_ProvinceId",
                schema: "BaseInformation",
                table: "Cities",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_PEOPLE_NATIONALCODE",
                schema: "People",
                table: "People",
                column: "NationalCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PeopleAddresses_CityId",
                schema: "People",
                table: "PeopleAddresses",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_PeopleAddresses_PersonId",
                schema: "People",
                table: "PeopleAddresses",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_PeopleAddresses_ProvinceId",
                schema: "People",
                table: "PeopleAddresses",
                column: "ProvinceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OutBoxEvents",
                schema: "Outbox");

            migrationBuilder.DropTable(
                name: "PeopleAddresses",
                schema: "People");

            migrationBuilder.DropTable(
                name: "Cities",
                schema: "BaseInformation");

            migrationBuilder.DropTable(
                name: "People",
                schema: "People");

            migrationBuilder.DropTable(
                name: "Provinces",
                schema: "BaseInformation");

            migrationBuilder.DropSequence(
                name: "Cities");

            migrationBuilder.DropSequence(
                name: "People");

            migrationBuilder.DropSequence(
                name: "Provinces");
        }
    }
}
