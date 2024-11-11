using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddClientFriendsRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientFriends",
                columns: table => new
                {
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    FriendId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientFriends", x => new { x.ClientId, x.FriendId });
                    table.ForeignKey(
                        name: "FK_ClientFriends_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientFriends_Clients_FriendId",
                        column: x => x.FriendId,
                        principalTable: "Clients",
                        principalColumn: "ClientId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientFriends_FriendId",
                table: "ClientFriends",
                column: "FriendId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientFriends");
        }
    }
}
