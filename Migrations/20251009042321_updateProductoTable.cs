using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace promociones.Migrations
{
    /// <inheritdoc />
    public partial class updateProductoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id_Promocion",
                table: "Productos",
                newName: "IdPromocion");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Productos",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Imagen",
                table: "Productos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TienePromocion",
                table: "Productos",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Imagen",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "TienePromocion",
                table: "Productos");

            migrationBuilder.RenameColumn(
                name: "IdPromocion",
                table: "Productos",
                newName: "Id_Promocion");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Productos",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
