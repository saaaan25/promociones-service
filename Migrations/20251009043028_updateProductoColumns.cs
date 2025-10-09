using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace promociones.Migrations
{
    /// <inheritdoc />
    public partial class updateProductoColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Imagen",
                table: "Productos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Imagen",
                table: "Productos",
                type: "text",
                nullable: true);
        }
    }
}
