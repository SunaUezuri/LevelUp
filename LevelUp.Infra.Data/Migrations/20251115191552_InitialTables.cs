using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LevelUp.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_LEVELUP_REWARDS",
                columns: table => new
                {
                    REWARD_ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NAME = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    DESCRIPTION = table.Column<string>(type: "NVARCHAR2(1000)", maxLength: 1000, nullable: true),
                    POINT_COST = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    STOCK_QUANTITY = table.Column<int>(type: "NUMBER(10)", nullable: false, defaultValue: 0),
                    CREATED_AT = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false, defaultValueSql: "SYSDATE"),
                    UPDATED_AT = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_ACTIVE = table.Column<string>(type: "CHAR(1)", nullable: false, defaultValue: "Y")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_LEVELUP_REWARDS", x => x.REWARD_ID);
                });

            migrationBuilder.CreateTable(
                name: "TB_LEVELUP_TEAMS",
                columns: table => new
                {
                    TEAM_ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    TEAM_NAME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_LEVELUP_TEAMS", x => x.TEAM_ID);
                });

            migrationBuilder.CreateTable(
                name: "TB_LEVELUP_USERS",
                columns: table => new
                {
                    USER_ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    FULL_NAME = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    EMAIL = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    PASSWORD_HASH = table.Column<string>(type: "NVARCHAR2(1000)", maxLength: 1000, nullable: false),
                    JOB_TITLE = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: true),
                    POINT_BALANCE = table.Column<int>(type: "NUMBER(10)", nullable: false, defaultValue: 0),
                    TEAM_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    ROLE = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false, defaultValue: "USER"),
                    CREATED_AT = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false, defaultValueSql: "SYSDATE"),
                    UPDATED_AT = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_ACTIVE = table.Column<string>(type: "CHAR(1)", nullable: false, defaultValue: "Y"),
                    TEAM_ID1 = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_LEVELUP_USERS", x => x.USER_ID);
                    table.ForeignKey(
                        name: "FK_TB_LEVELUP_USERS_TB_LEVELUP_TEAMS_TEAM_ID1",
                        column: x => x.TEAM_ID1,
                        principalTable: "TB_LEVELUP_TEAMS",
                        principalColumn: "TEAM_ID");
                });

            migrationBuilder.CreateTable(
                name: "TB_LEVELUP_REWARD_REDEMPTIONS",
                columns: table => new
                {
                    REDEMPTION_ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    USER_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    REWARD_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    REDEEMED_AT = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    POINTS_SPENT = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    REWARD_ID1 = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    USER_ID1 = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_LEVELUP_REWARD_REDEMPTIONS", x => x.REDEMPTION_ID);
                    table.ForeignKey(
                        name: "FK_REDEMPTIONS_REWARD",
                        column: x => x.REWARD_ID,
                        principalTable: "TB_LEVELUP_REWARDS",
                        principalColumn: "REWARD_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_REDEMPTIONS_USER",
                        column: x => x.USER_ID,
                        principalTable: "TB_LEVELUP_USERS",
                        principalColumn: "USER_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_LEVELUP_REWARD_REDEMPTIONS_REWARD_ID",
                table: "TB_LEVELUP_REWARD_REDEMPTIONS",
                column: "REWARD_ID");

            migrationBuilder.CreateIndex(
                name: "IX_TB_LEVELUP_REWARD_REDEMPTIONS_USER_ID",
                table: "TB_LEVELUP_REWARD_REDEMPTIONS",
                column: "USER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_TB_LEVELUP_TEAMS_TEAM_NAME",
                table: "TB_LEVELUP_TEAMS",
                column: "TEAM_NAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_LEVELUP_USERS_EMAIL",
                table: "TB_LEVELUP_USERS",
                column: "EMAIL",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_LEVELUP_USERS_TEAM_ID1",
                table: "TB_LEVELUP_USERS",
                column: "TEAM_ID1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_LEVELUP_REWARD_REDEMPTIONS");

            migrationBuilder.DropTable(
                name: "TB_LEVELUP_REWARDS");

            migrationBuilder.DropTable(
                name: "TB_LEVELUP_USERS");

            migrationBuilder.DropTable(
                name: "TB_LEVELUP_TEAMS");
        }
    }
}
