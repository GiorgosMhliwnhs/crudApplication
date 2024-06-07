using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace peripatoiCrud.API.Migrations
{
    /// <inheritdoc />
    public partial class KataxwrishDedomenvngiadyskolieskaiperioxes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Dyskolies",
                columns: new[] { "Id", "Onoma" },
                values: new object[,]
                {
                    { new Guid("09061773-9946-4c79-804e-0f33f6c23213"), "EYKOLOS" },
                    { new Guid("ca2f6118-2f8f-4ca5-99f5-31287e3dcf15"), "DYSKOLOS" },
                    { new Guid("f4db66cf-1936-48ff-b8e7-8c99701bcfd9"), "METRIOS" }
                });

            migrationBuilder.InsertData(
                table: "Perioxes",
                columns: new[] { "Id", "EikonaUrl", "Kwdikos", "Onoma" },
                values: new object[,]
                {
                    { new Guid("367e8f8a-6a1b-4d44-b6b8-9366e21bbb83"), "https://images.pexels.com/photos/3327997/pexels-photo-3327997.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1", "RHO", "RODOS" },
                    { new Guid("5a1cc102-9bbf-4818-aa76-52d7ebdcff21"), "https://images.pexels.com/photos/17505177/pexels-photo-17505177/free-photo-of-white-tower-in-thessaloniki.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1", "THS", "THESSALONIKI" },
                    { new Guid("79816b96-50ac-43eb-8b74-9407fb54c613"), "https://images.pexels.com/photos/13861594/pexels-photo-13861594.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1", "CRE", "KRHTH" },
                    { new Guid("858e610e-fa5e-47dc-9ec4-38af459e1646"), "https://media.istockphoto.com/id/1028749698/photo/the-acropolis-of-athens-greece.jpg?s=2048x2048&w=is&k=20&c=FMjbIryuJtIGU83EaaBafEDW49KjhcnUA3TYHfKZWYs=", "ATH", "ATHINA" },
                    { new Guid("d96fe13d-5c86-4425-8075-0c9d3e062d81"), "https://images.pexels.com/photos/10400151/pexels-photo-10400151.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1", "NFP", "NAFPLIO" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Dyskolies",
                keyColumn: "Id",
                keyValue: new Guid("09061773-9946-4c79-804e-0f33f6c23213"));

            migrationBuilder.DeleteData(
                table: "Dyskolies",
                keyColumn: "Id",
                keyValue: new Guid("ca2f6118-2f8f-4ca5-99f5-31287e3dcf15"));

            migrationBuilder.DeleteData(
                table: "Dyskolies",
                keyColumn: "Id",
                keyValue: new Guid("f4db66cf-1936-48ff-b8e7-8c99701bcfd9"));

            migrationBuilder.DeleteData(
                table: "Perioxes",
                keyColumn: "Id",
                keyValue: new Guid("367e8f8a-6a1b-4d44-b6b8-9366e21bbb83"));

            migrationBuilder.DeleteData(
                table: "Perioxes",
                keyColumn: "Id",
                keyValue: new Guid("5a1cc102-9bbf-4818-aa76-52d7ebdcff21"));

            migrationBuilder.DeleteData(
                table: "Perioxes",
                keyColumn: "Id",
                keyValue: new Guid("79816b96-50ac-43eb-8b74-9407fb54c613"));

            migrationBuilder.DeleteData(
                table: "Perioxes",
                keyColumn: "Id",
                keyValue: new Guid("858e610e-fa5e-47dc-9ec4-38af459e1646"));

            migrationBuilder.DeleteData(
                table: "Perioxes",
                keyColumn: "Id",
                keyValue: new Guid("d96fe13d-5c86-4425-8075-0c9d3e062d81"));
        }
    }
}
