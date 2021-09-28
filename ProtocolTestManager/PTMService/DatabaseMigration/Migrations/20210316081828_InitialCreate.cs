// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.EntityFrameworkCore.Migrations;

namespace Microsoft.Protocols.TestManager.PTMService.DatabaseMigration.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestSuiteInstallations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Version = table.Column<string>(type: "TEXT", nullable: true),
                    InstallMethod = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Path = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestSuiteInstallations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TestSuiteConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    TestSuiteId = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Path = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestSuiteConfigurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestSuiteConfigurations_TestSuiteInstallations_TestSuiteId",
                        column: x => x.TestSuiteId,
                        principalTable: "TestSuiteInstallations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TestSuiteConfigurationId = table.Column<int>(type: "INTEGER", nullable: false),
                    State = table.Column<int>(type: "INTEGER", nullable: false),
                    Path = table.Column<string>(type: "TEXT", nullable: true),
                    Total = table.Column<int>(type: "INTEGER", nullable: true),
                    NotRun = table.Column<int>(type: "INTEGER", nullable: true),
                    Passed = table.Column<int>(type: "INTEGER", nullable: true),
                    Failed = table.Column<int>(type: "INTEGER", nullable: true),
                    Inconclusive = table.Column<int>(type: "INTEGER", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestResults_TestSuiteConfigurations_TestSuiteConfigurationId",
                        column: x => x.TestSuiteConfigurationId,
                        principalTable: "TestSuiteConfigurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestResults_TestSuiteConfigurationId",
                table: "TestResults",
                column: "TestSuiteConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_TestSuiteConfigurations_TestSuiteId",
                table: "TestSuiteConfigurations",
                column: "TestSuiteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestResults");

            migrationBuilder.DropTable(
                name: "TestSuiteConfigurations");

            migrationBuilder.DropTable(
                name: "TestSuiteInstallations");
        }
    }
}
