using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockFlow.Repository.Migrations
{
    /// <inheritdoc />
    public partial class madeChangesInBaseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // user_roles
            migrationBuilder.Sql(
                "ALTER TABLE user_roles ALTER COLUMN \"ModifiedBy\" TYPE uuid USING \"ModifiedBy\"::text::uuid;");
            migrationBuilder.Sql(
                "ALTER TABLE user_roles ALTER COLUMN \"CreatedBy\" TYPE uuid USING \"CreatedBy\"::text::uuid;");

            // user_auth
            migrationBuilder.Sql(
                "ALTER TABLE user_auth ALTER COLUMN \"ModifiedBy\" TYPE uuid USING \"ModifiedBy\"::text::uuid;");
            migrationBuilder.Sql(
                "ALTER TABLE user_auth ALTER COLUMN \"CreatedBy\" TYPE uuid USING \"CreatedBy\"::text::uuid;");

            // roles
            migrationBuilder.Sql(
                "ALTER TABLE roles ALTER COLUMN \"ModifiedBy\" TYPE uuid USING \"ModifiedBy\"::text::uuid;");
            migrationBuilder.Sql(
                "ALTER TABLE roles ALTER COLUMN \"CreatedBy\" TYPE uuid USING \"CreatedBy\"::text::uuid;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert UUIDs back to int if needed
            migrationBuilder.Sql(
                "ALTER TABLE user_roles ALTER COLUMN \"ModifiedBy\" TYPE integer USING \"ModifiedBy\"::text::integer;");
            migrationBuilder.Sql(
                "ALTER TABLE user_roles ALTER COLUMN \"CreatedBy\" TYPE integer USING \"CreatedBy\"::text::integer;");

            migrationBuilder.Sql(
                "ALTER TABLE user_auth ALTER COLUMN \"ModifiedBy\" TYPE integer USING \"ModifiedBy\"::text::integer;");
            migrationBuilder.Sql(
                "ALTER TABLE user_auth ALTER COLUMN \"CreatedBy\" TYPE integer USING \"CreatedBy\"::text::integer;");

            migrationBuilder.Sql(
                "ALTER TABLE roles ALTER COLUMN \"ModifiedBy\" TYPE integer USING \"ModifiedBy\"::text::integer;");
            migrationBuilder.Sql(
                "ALTER TABLE roles ALTER COLUMN \"CreatedBy\" TYPE integer USING \"CreatedBy\"::text::integer;");
        }
    }
}
