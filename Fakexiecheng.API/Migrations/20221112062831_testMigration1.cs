using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fakexiecheng.API.Migrations
{
    public partial class testMigration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Form_TD",
                columns: table => new
                {
                    ford_order_guid = table.Column<Guid>(nullable: false),
                    C_Addition_Date = table.Column<DateTime>(nullable: false),
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(nullable: true),
                    mobile = table.Column<string>(nullable: true),
                    channel = table.Column<string>(nullable: true),
                    channel_code = table.Column<string>(nullable: true),
                    channel_comment = table.Column<string>(nullable: true),
                    active = table.Column<string>(nullable: true),
                    act_code = table.Column<string>(nullable: true),
                    purchase_intention = table.Column<int>(nullable: false),
                    purchase_time = table.Column<DateTime>(nullable: true),
                    business_type = table.Column<int>(nullable: false),
                    td_order = table.Column<string>(nullable: true),
                    td_start_time = table.Column<DateTime>(nullable: true),
                    td_end_time = table.Column<DateTime>(nullable: true),
                    td_duration = table.Column<string>(nullable: true),
                    td_city = table.Column<string>(nullable: true),
                    td_distance = table.Column<string>(nullable: true),
                    td_evaluate = table.Column<string>(nullable: true),
                    td_merit = table.Column<string>(nullable: true),
                    td_purchase_date = table.Column<string>(nullable: true),
                    td_comment = table.Column<string>(nullable: true),
                    reg_info_completed = table.Column<bool>(nullable: false),
                    update_td_user_info_completed = table.Column<bool>(nullable: false),
                    update_td_order_info_completed = table.Column<bool>(nullable: false),
                    update_td_info_completed = table.Column<bool>(nullable: false),
                    req_td_info_completed = table.Column<bool>(nullable: false),
                    sync_td_info_completed = table.Column<bool>(nullable: false),
                    update_td_user_info_complete_at = table.Column<DateTime>(nullable: false),
                    reg_info_complete_at = table.Column<DateTime>(nullable: false),
                    update_td_order_info_complete_at = table.Column<DateTime>(nullable: false),
                    update_td_info_complete_at = table.Column<DateTime>(nullable: false),
                    req_td_info_complete_at = table.Column<DateTime>(nullable: false),
                    sync_td_info_complete_at = table.Column<DateTime>(nullable: false),
                    customer_wx_weapp_openid = table.Column<string>(nullable: true),
                    state_current = table.Column<string>(nullable: true),
                    state_name_current = table.Column<string>(nullable: true),
                    state_next = table.Column<string>(nullable: true),
                    state_name_next = table.Column<string>(nullable: true),
                    request_result = table.Column<string>(nullable: true),
                    sync_is_success = table.Column<bool>(nullable: false),
                    intention_province = table.Column<string>(nullable: true),
                    intention_city = table.Column<string>(nullable: true),
                    is_connect_success = table.Column<bool>(nullable: false),
                    connect_success_at = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Form_TD", x => x.ford_order_guid);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "308660dc-ae51-480f-824d-7dca6714c3e2",
                column: "ConcurrencyStamp",
                value: "0fa5a243-5d01-49d4-9894-ec373ff2fe7f");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "90184155-dee0-40c9-bb1e-b5ed07afc04e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2545cc4e-8ad2-48e6-8901-72652b836f10", "AQAAAAEAACcQAAAAEBwMh3pLiPYUEewg/Ku7Wrpx9Vq/IGCjpo1ShJEwGaRY2rib+8kBtGVwB5GAa1lXvQ==", "bb0239b8-ef7b-4aec-99f2-185d4fd7c5b4" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Form_TD");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "308660dc-ae51-480f-824d-7dca6714c3e2",
                column: "ConcurrencyStamp",
                value: "d077361e-dd38-47d5-b247-436d8450f96c");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "90184155-dee0-40c9-bb1e-b5ed07afc04e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "04d384d2-4312-4d0e-9eaa-5b1ac0067bab", "AQAAAAEAACcQAAAAEO5laNO+u+osxjrbEYDunjJGHi98GwPUhlMlgkUY9hYrlYj9cA5M2/elut9W0QisKg==", "13b833a7-3c1c-4fff-8909-17e27a7573fa" });
        }
    }
}
