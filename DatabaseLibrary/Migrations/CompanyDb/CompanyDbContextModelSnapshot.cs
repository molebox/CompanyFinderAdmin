﻿// <auto-generated />
using DatabaseLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace DatabaseLibrary.Migrations.CompanyDb
{
    [DbContext(typeof(CompanyDbContext))]
    partial class CompanyDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DatabaseLibrary.Models.Companies", b =>
                {
                    b.Property<int>("CompanyId");

                    b.Property<string>("Bio");

                    b.Property<string>("CompanyName");

                    b.Property<string>("ContactPerson");

                    b.Property<string>("Email");

                    b.Property<int?>("FocusId");

                    b.Property<string>("Phone");

                    b.Property<int?>("SkillDetailId");

                    b.Property<int?>("SkillSetSkillId");

                    b.Property<string>("Website");

                    b.HasKey("CompanyId");

                    b.HasIndex("FocusId");

                    b.HasIndex("SkillDetailId");

                    b.HasIndex("SkillSetSkillId");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("DatabaseLibrary.Models.CompanyDetails", b =>
                {
                    b.Property<int>("CompanyDetailsId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CompanyId");

                    b.Property<int>("SkillDetailId");

                    b.HasKey("CompanyDetailsId");

                    b.HasIndex("CompanyId");

                    b.HasIndex("SkillDetailId");

                    b.ToTable("CompanyDetails");
                });

            modelBuilder.Entity("DatabaseLibrary.Models.CompanyFocus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CompanyId");

                    b.Property<int>("FocusId");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("FocusId");

                    b.ToTable("CompanyFocus");
                });

            modelBuilder.Entity("DatabaseLibrary.Models.CompanySkills", b =>
                {
                    b.Property<int>("CompSkillId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CompanyId");

                    b.Property<int>("SkillId");

                    b.HasKey("CompSkillId");

                    b.HasIndex("CompanyId");

                    b.HasIndex("SkillId");

                    b.ToTable("CompanySkills");
                });

            modelBuilder.Entity("DatabaseLibrary.Models.Focus", b =>
                {
                    b.Property<int>("FocusId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FocusType");

                    b.HasKey("FocusId");

                    b.ToTable("Focus");
                });

            modelBuilder.Entity("DatabaseLibrary.Models.HomePage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("TagLine");

                    b.HasKey("Id");

                    b.ToTable("HomePage");
                });

            modelBuilder.Entity("DatabaseLibrary.Models.SkillDetail", b =>
                {
                    b.Property<int>("SkillDetailId");

                    b.Property<string>("DetailName");

                    b.HasKey("SkillDetailId");

                    b.ToTable("SkillDetail");
                });

            modelBuilder.Entity("DatabaseLibrary.Models.SkillSet", b =>
                {
                    b.Property<int>("SkillId");

                    b.Property<string>("SkillName");

                    b.HasKey("SkillId");

                    b.ToTable("SkillSet");
                });

            modelBuilder.Entity("DatabaseLibrary.Models.TemporaryCompanyDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CompanyId");

                    b.Property<int>("SkillDetailId");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("SkillDetailId");

                    b.ToTable("TemporaryCompanyDetails");
                });

            modelBuilder.Entity("DatabaseLibrary.Models.TemporaryCompanyFocus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CompanyId");

                    b.Property<int>("FocusId");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("FocusId");

                    b.ToTable("TemporaryCompanyFocus");
                });

            modelBuilder.Entity("DatabaseLibrary.Models.TemporaryCompanySkills", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CompanyId");

                    b.Property<int>("SkillId");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("SkillId");

                    b.ToTable("TemporaryCompanySkills");
                });

            modelBuilder.Entity("DatabaseLibrary.Models.TemporaryCompanyTemplate", b =>
                {
                    b.Property<int>("CompanyId");

                    b.Property<string>("Bio");

                    b.Property<Guid>("CompanyGuid");

                    b.Property<string>("CompanyName");

                    b.Property<string>("ContactPerson");

                    b.Property<DateTime?>("Created_at");

                    b.Property<string>("Email");

                    b.Property<bool>("Locked");

                    b.Property<string>("OtherNotes");

                    b.Property<string>("Phone");

                    b.Property<DateTime?>("Received_at");

                    b.Property<string>("RecruitmentWebAddress");

                    b.Property<DateTime>("Sent_at");

                    b.Property<string>("Website");

                    b.HasKey("CompanyId");

                    b.ToTable("TemporaryCompanyTemplate");
                });

            modelBuilder.Entity("DatabaseLibrary.Models.Companies", b =>
                {
                    b.HasOne("DatabaseLibrary.Models.Focus")
                        .WithMany("Companies")
                        .HasForeignKey("FocusId");

                    b.HasOne("DatabaseLibrary.Models.SkillDetail")
                        .WithMany("Companies")
                        .HasForeignKey("SkillDetailId");

                    b.HasOne("DatabaseLibrary.Models.SkillSet")
                        .WithMany("Companies")
                        .HasForeignKey("SkillSetSkillId");
                });

            modelBuilder.Entity("DatabaseLibrary.Models.CompanyDetails", b =>
                {
                    b.HasOne("DatabaseLibrary.Models.Companies", "Company")
                        .WithMany("CompanyDetails")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DatabaseLibrary.Models.SkillDetail", "SkillDetail")
                        .WithMany()
                        .HasForeignKey("SkillDetailId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DatabaseLibrary.Models.CompanyFocus", b =>
                {
                    b.HasOne("DatabaseLibrary.Models.Companies", "Company")
                        .WithMany("CompanyFocuses")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DatabaseLibrary.Models.Focus", "Focus")
                        .WithMany()
                        .HasForeignKey("FocusId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DatabaseLibrary.Models.CompanySkills", b =>
                {
                    b.HasOne("DatabaseLibrary.Models.Companies", "Company")
                        .WithMany("CompanySkills")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DatabaseLibrary.Models.SkillSet", "SkillSet")
                        .WithMany()
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DatabaseLibrary.Models.TemporaryCompanyDetails", b =>
                {
                    b.HasOne("DatabaseLibrary.Models.TemporaryCompanyTemplate", "Company")
                        .WithMany("TemporaryCompanyDetails")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DatabaseLibrary.Models.SkillDetail", "SkillDetail")
                        .WithMany()
                        .HasForeignKey("SkillDetailId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DatabaseLibrary.Models.TemporaryCompanyFocus", b =>
                {
                    b.HasOne("DatabaseLibrary.Models.TemporaryCompanyTemplate", "Company")
                        .WithMany("TemporaryCompanyFocuses")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DatabaseLibrary.Models.Focus", "Focus")
                        .WithMany()
                        .HasForeignKey("FocusId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DatabaseLibrary.Models.TemporaryCompanySkills", b =>
                {
                    b.HasOne("DatabaseLibrary.Models.TemporaryCompanyTemplate", "Company")
                        .WithMany("TemporaryCompanySkills")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DatabaseLibrary.Models.SkillSet", "SkillSet")
                        .WithMany()
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
