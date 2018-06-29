using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DatabaseLibrary.Migrations.CompanyDb
{
    public partial class TemporaryTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "Focus",
            //    columns: table => new
            //    {
            //        FocusId = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        FocusType = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Focus", x => x.FocusId);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "HomePage",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        TagLine = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_HomePage", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "SkillDetail",
            //    columns: table => new
            //    {
            //        SkillDetailId = table.Column<int>(nullable: false),
            //        DetailName = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_SkillDetail", x => x.SkillDetailId);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "SkillSet",
            //    columns: table => new
            //    {
            //        SkillId = table.Column<int>(nullable: false),
            //        SkillName = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_SkillSet", x => x.SkillId);
            //    });

            migrationBuilder.CreateTable(
                name: "TemporaryCompanyTemplate",
                columns: table => new
                {
                    CompanyId = table.Column<int>(nullable: false),
                    Bio = table.Column<string>(nullable: true),
                    CompanyGuid = table.Column<Guid>(nullable: false),
                    CompanyName = table.Column<string>(nullable: true),
                    ContactPerson = table.Column<string>(nullable: true),
                    Created_at = table.Column<DateTime>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Locked = table.Column<bool>(nullable: false),
                    OtherNotes = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Received_at = table.Column<DateTime>(nullable: true),
                    RecruitmentWebAddress = table.Column<string>(nullable: true),
                    Sent_at = table.Column<DateTime>(nullable: false),
                    Website = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemporaryCompanyTemplate", x => x.CompanyId);
                });

            //migrationBuilder.CreateTable(
            //    name: "Companies",
            //    columns: table => new
            //    {
            //        CompanyId = table.Column<int>(nullable: false),
            //        Bio = table.Column<string>(nullable: true),
            //        CompanyName = table.Column<string>(nullable: true),
            //        ContactPerson = table.Column<string>(nullable: true),
            //        Email = table.Column<string>(nullable: true),
            //        FocusId = table.Column<int>(nullable: true),
            //        Phone = table.Column<string>(nullable: true),
            //        SkillDetailId = table.Column<int>(nullable: true),
            //        SkillSetSkillId = table.Column<int>(nullable: true),
            //        Website = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Companies", x => x.CompanyId);
            //        table.ForeignKey(
            //            name: "FK_Companies_Focus_FocusId",
            //            column: x => x.FocusId,
            //            principalTable: "Focus",
            //            principalColumn: "FocusId",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Companies_SkillDetail_SkillDetailId",
            //            column: x => x.SkillDetailId,
            //            principalTable: "SkillDetail",
            //            principalColumn: "SkillDetailId",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Companies_SkillSet_SkillSetSkillId",
            //            column: x => x.SkillSetSkillId,
            //            principalTable: "SkillSet",
            //            principalColumn: "SkillId",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            migrationBuilder.CreateTable(
                name: "TemporaryCompanyDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<int>(nullable: false),
                    SkillDetailId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemporaryCompanyDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemporaryCompanyDetails_TemporaryCompanyTemplate_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "TemporaryCompanyTemplate",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TemporaryCompanyDetails_SkillDetail_SkillDetailId",
                        column: x => x.SkillDetailId,
                        principalTable: "SkillDetail",
                        principalColumn: "SkillDetailId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TemporaryCompanyFocus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<int>(nullable: false),
                    FocusId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemporaryCompanyFocus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemporaryCompanyFocus_TemporaryCompanyTemplate_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "TemporaryCompanyTemplate",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TemporaryCompanyFocus_Focus_FocusId",
                        column: x => x.FocusId,
                        principalTable: "Focus",
                        principalColumn: "FocusId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TemporaryCompanySkills",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<int>(nullable: false),
                    SkillId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemporaryCompanySkills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemporaryCompanySkills_TemporaryCompanyTemplate_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "TemporaryCompanyTemplate",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TemporaryCompanySkills_SkillSet_SkillId",
                        column: x => x.SkillId,
                        principalTable: "SkillSet",
                        principalColumn: "SkillId",
                        onDelete: ReferentialAction.Cascade);
                });

            //migrationBuilder.CreateTable(
            //    name: "CompanyDetails",
            //    columns: table => new
            //    {
            //        CompanyDetailsId = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        CompanyId = table.Column<int>(nullable: false),
            //        SkillDetailId = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_CompanyDetails", x => x.CompanyDetailsId);
            //        table.ForeignKey(
            //            name: "FK_CompanyDetails_Companies_CompanyId",
            //            column: x => x.CompanyId,
            //            principalTable: "Companies",
            //            principalColumn: "CompanyId",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_CompanyDetails_SkillDetail_SkillDetailId",
            //            column: x => x.SkillDetailId,
            //            principalTable: "SkillDetail",
            //            principalColumn: "SkillDetailId",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "CompanyFocus",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        CompanyId = table.Column<int>(nullable: false),
            //        FocusId = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_CompanyFocus", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_CompanyFocus_Companies_CompanyId",
            //            column: x => x.CompanyId,
            //            principalTable: "Companies",
            //            principalColumn: "CompanyId",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_CompanyFocus_Focus_FocusId",
            //            column: x => x.FocusId,
            //            principalTable: "Focus",
            //            principalColumn: "FocusId",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "CompanySkills",
            //    columns: table => new
            //    {
            //        CompSkillId = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        CompanyId = table.Column<int>(nullable: false),
            //        SkillId = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_CompanySkills", x => x.CompSkillId);
            //        table.ForeignKey(
            //            name: "FK_CompanySkills_Companies_CompanyId",
            //            column: x => x.CompanyId,
            //            principalTable: "Companies",
            //            principalColumn: "CompanyId",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_CompanySkills_SkillSet_SkillId",
            //            column: x => x.SkillId,
            //            principalTable: "SkillSet",
            //            principalColumn: "SkillId",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Companies_FocusId",
            //    table: "Companies",
            //    column: "FocusId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Companies_SkillDetailId",
            //    table: "Companies",
            //    column: "SkillDetailId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Companies_SkillSetSkillId",
            //    table: "Companies",
            //    column: "SkillSetSkillId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_CompanyDetails_CompanyId",
            //    table: "CompanyDetails",
            //    column: "CompanyId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_CompanyDetails_SkillDetailId",
            //    table: "CompanyDetails",
            //    column: "SkillDetailId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_CompanyFocus_CompanyId",
            //    table: "CompanyFocus",
            //    column: "CompanyId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_CompanyFocus_FocusId",
            //    table: "CompanyFocus",
            //    column: "FocusId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_CompanySkills_CompanyId",
            //    table: "CompanySkills",
            //    column: "CompanyId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_CompanySkills_SkillId",
            //    table: "CompanySkills",
            //    column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_TemporaryCompanyDetails_CompanyId",
                table: "TemporaryCompanyDetails",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TemporaryCompanyDetails_SkillDetailId",
                table: "TemporaryCompanyDetails",
                column: "SkillDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_TemporaryCompanyFocus_CompanyId",
                table: "TemporaryCompanyFocus",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TemporaryCompanyFocus_FocusId",
                table: "TemporaryCompanyFocus",
                column: "FocusId");

            migrationBuilder.CreateIndex(
                name: "IX_TemporaryCompanySkills_CompanyId",
                table: "TemporaryCompanySkills",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TemporaryCompanySkills_SkillId",
                table: "TemporaryCompanySkills",
                column: "SkillId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyDetails");

            migrationBuilder.DropTable(
                name: "CompanyFocus");

            migrationBuilder.DropTable(
                name: "CompanySkills");

            migrationBuilder.DropTable(
                name: "HomePage");

            migrationBuilder.DropTable(
                name: "TemporaryCompanyDetails");

            migrationBuilder.DropTable(
                name: "TemporaryCompanyFocus");

            migrationBuilder.DropTable(
                name: "TemporaryCompanySkills");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "TemporaryCompanyTemplate");

            migrationBuilder.DropTable(
                name: "Focus");

            migrationBuilder.DropTable(
                name: "SkillDetail");

            migrationBuilder.DropTable(
                name: "SkillSet");
        }
    }
}
