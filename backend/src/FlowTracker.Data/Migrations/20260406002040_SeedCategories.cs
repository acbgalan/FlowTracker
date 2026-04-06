using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FlowTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Icon", "Name", "Type", "UserId" },
                values: new object[,]
                {
                    { 1, "Alquiler, hipoteca, comunidad.", "default-icon", "Vivienda", "Expense", null },
                    { 2, "Luz, agua, gas, internet, basura.", "default-icon", "Servicios", "Expense", null },
                    { 3, "Comida, productos de limpieza, aseo personal.", "default-icon", "Supermercado", "Expense", null },
                    { 4, "Gasolina, transporte público, parking, mantenimiento coche.", "default-icon", "Transporte", "Expense", null },
                    { 5, "Farmacia, seguro médico, dentista.", "default-icon", "Salud", "Expense", null },
                    { 6, "Restaurantes, bares, café, comida a domicilio.", "default-icon", "Restauración", "Expense", null },
                    { 7, "Cine, conciertos, suscripciones (Netflix, Spotify, DAZN).", "default-icon", "Ocio", "Expense", null },
                    { 8, "Ropa, calzado, accesorios, electrónica.", "default-icon", "Compras", "Expense", null },
                    { 9, "Hoteles, vuelos, escapadas de fin de semana.", "default-icon", "Viajes", "Expense", null },
                    { 10, "Gimnasio, peluquería, estética.", "default-icon", "Cuidado Personal", "Expense", null },
                    { 11, "El ingreso principal.", "default-icon", "Nómina/Salario", "Income", null },
                    { 12, "Trabajos extra o proyectos secundarios.", "default-icon", "Freelance", "Income", null },
                    { 13, "Dinero recibido por cumpleaños o eventos.", "default-icon", "Regalos", "Income", null },
                    { 14, "Dividendos, intereses bancarios, cripto.", "default-icon", "Inversiones", "Income", null },
                    { 15, "Dinero por vender artículos usados (Wallapop, Vinted).", "default-icon", "Venta", "Income", null },
                    { 16, "Dinero que se mueve de la cuenta corriente a una hucha.", "default-icon", "Ahorro e inversión", "Saving", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 16);
        }
    }
}
