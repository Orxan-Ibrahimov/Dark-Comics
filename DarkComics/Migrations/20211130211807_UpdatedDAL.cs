using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DarkComics.Migrations
{
    public partial class UpdatedDAL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DeactivatedDate",
                table: "Toys",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "Date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Toys",
                nullable: false,
                defaultValueSql: "dateadd(hour,4,getutcdate())",
                oldClrType: typeof(DateTime),
                oldType: "Date",
                oldDefaultValueSql: "dateadd(hour,4,getutcdate())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Series",
                nullable: false,
                defaultValueSql: "dateadd(hour,4,getutcdate())",
                oldClrType: typeof(DateTime),
                oldType: "Date",
                oldDefaultValueSql: "dateadd(hour,4,getutcdate())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Sales",
                nullable: false,
                defaultValueSql: "dateadd(hour,4,getutcdate())",
                oldClrType: typeof(DateTime),
                oldType: "Date",
                oldDefaultValueSql: "dateadd(hour,4,getutcdate())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDay",
                table: "Sales",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "Date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ReadingComics",
                nullable: false,
                defaultValueSql: "dateadd(hour,4,getutcdate())",
                oldClrType: typeof(DateTime),
                oldType: "Date",
                oldDefaultValueSql: "dateadd(hour,4,getutcdate())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeActivatedDate",
                table: "Products",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "Date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Products",
                nullable: false,
                defaultValueSql: "dateadd(hour,4,getutcdate())",
                oldClrType: typeof(DateTime),
                oldType: "Date",
                oldDefaultValueSql: "dateadd(hour,4,getutcdate())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "News",
                nullable: true,
                defaultValueSql: "dateadd(hour,4,getutcdate())",
                oldClrType: typeof(DateTime),
                oldType: "Date",
                oldNullable: true,
                oldDefaultValueSql: "dateadd(hour,4,getutcdate())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Cities",
                nullable: false,
                defaultValueSql: "dateadd(hour,4,getutcdate())",
                oldClrType: typeof(DateTime),
                oldType: "Date",
                oldDefaultValueSql: "dateadd(hour,4,getutcdate())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Characters",
                nullable: false,
                defaultValueSql: "dateadd(hour,4,getutcdate())",
                oldClrType: typeof(DateTime),
                oldType: "Date",
                oldDefaultValueSql: "dateadd(hour,4,getutcdate())");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DeactivatedDate",
                table: "Toys",
                type: "Date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Toys",
                type: "Date",
                nullable: false,
                defaultValueSql: "dateadd(hour,4,getutcdate())",
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "dateadd(hour,4,getutcdate())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Series",
                type: "Date",
                nullable: false,
                defaultValueSql: "dateadd(hour,4,getutcdate())",
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "dateadd(hour,4,getutcdate())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Sales",
                type: "Date",
                nullable: false,
                defaultValueSql: "dateadd(hour,4,getutcdate())",
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "dateadd(hour,4,getutcdate())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDay",
                table: "Sales",
                type: "Date",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ReadingComics",
                type: "Date",
                nullable: false,
                defaultValueSql: "dateadd(hour,4,getutcdate())",
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "dateadd(hour,4,getutcdate())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeActivatedDate",
                table: "Products",
                type: "Date",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Products",
                type: "Date",
                nullable: false,
                defaultValueSql: "dateadd(hour,4,getutcdate())",
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "dateadd(hour,4,getutcdate())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "News",
                type: "Date",
                nullable: true,
                defaultValueSql: "dateadd(hour,4,getutcdate())",
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValueSql: "dateadd(hour,4,getutcdate())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Cities",
                type: "Date",
                nullable: false,
                defaultValueSql: "dateadd(hour,4,getutcdate())",
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "dateadd(hour,4,getutcdate())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Characters",
                type: "Date",
                nullable: false,
                defaultValueSql: "dateadd(hour,4,getutcdate())",
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "dateadd(hour,4,getutcdate())");
        }
    }
}
