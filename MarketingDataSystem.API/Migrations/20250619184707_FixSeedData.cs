using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketingDataSystem.API.Migrations
{
    /// <inheritdoc />
    public partial class FixSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "UsuariosMarketing",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "PasswordHash" },
                values: new object[] { new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "$2a$11$8K1p/a0dclZhDnID4ZpOyeJ9pz26.EkwG/z7zKmFPQ/6hZv2K.nT." });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "UsuariosMarketing",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "PasswordHash" },
                values: new object[] { new DateTime(2025, 6, 19, 15, 46, 1, 893, DateTimeKind.Local).AddTicks(5446), "$2a$11$nf0Y8ETwacI6i1tmln3veeZoD8NEA0s/UC9Qc4XUDHlx7fxK4p52i" });
        }
    }
}
