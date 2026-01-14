using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieBuff.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserListItems_UserLists_UserFilmListId",
                table: "UserListItems");

            migrationBuilder.DropIndex(
                name: "IX_UserListItems_UserFilmListId",
                table: "UserListItems");

            migrationBuilder.DropColumn(
                name: "UserFilmListId",
                table: "UserListItems");

            migrationBuilder.CreateIndex(
                name: "IX_UserListItems_UserListId",
                table: "UserListItems",
                column: "UserListId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserListItems_UserLists_UserListId",
                table: "UserListItems",
                column: "UserListId",
                principalTable: "UserLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserListItems_UserLists_UserListId",
                table: "UserListItems");

            migrationBuilder.DropIndex(
                name: "IX_UserListItems_UserListId",
                table: "UserListItems");

            migrationBuilder.AddColumn<int>(
                name: "UserFilmListId",
                table: "UserListItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserListItems_UserFilmListId",
                table: "UserListItems",
                column: "UserFilmListId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserListItems_UserLists_UserFilmListId",
                table: "UserListItems",
                column: "UserFilmListId",
                principalTable: "UserLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
