using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MemoryTrave.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeleteArticlePhotos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EncryptedData",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "EncryptedPreviewData",
                table: "Articles");

            migrationBuilder.RenameColumn(
                name: "PhotosUrls",
                table: "Articles",
                newName: "EncryptedDescription");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EncryptedDescription",
                table: "Articles",
                newName: "PhotosUrls");

            migrationBuilder.AddColumn<string>(
                name: "EncryptedData",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EncryptedPreviewData",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
