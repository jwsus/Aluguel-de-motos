using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mottu.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          migrationBuilder.Sql(
            "INSERT INTO \"Users\" (\"Id\", \"UserName\", \"PasswordHash\", \"Role\") " +
            "VALUES ('c1a9511b-1c25-470f-8396-dfbc41eb69b4', 'adm', 'hvZeKKdU4acbLflANhWmxDbDLEKnWhDQKBOWG4bx5Cg=', 0)"
          );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
