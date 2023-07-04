﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using eAchizitii.Data;

#nullable disable

namespace eAchizitii.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20221231141317_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("eAchizitii.Models.AdresaLivrare", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Adresa")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SucursalaId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SucursalaId");

                    b.ToTable("AdreseLivrare");
                });

            modelBuilder.Entity("eAchizitii.Models.Angajat_comanda", b =>
                {
                    b.Property<string>("angajatId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("ComandaId")
                        .HasColumnType("int");

                    b.Property<int>("rolAngajatComanda")
                        .HasColumnType("int");

                    b.HasKey("angajatId", "ComandaId");

                    b.HasIndex("ComandaId");

                    b.ToTable("Angajati_comenzi");
                });

            modelBuilder.Entity("eAchizitii.Models.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Nume")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SucursalaId")
                        .HasColumnType("int");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("SucursalaId");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("eAchizitii.Models.Comanda", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AdresaLivrareId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DataComanda")
                        .HasColumnType("datetime2");

                    b.Property<string>("Observatii")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SucursalaId")
                        .HasColumnType("int");

                    b.Property<int>("status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AdresaLivrareId");

                    b.HasIndex("SucursalaId");

                    b.ToTable("Comenzi");
                });

            modelBuilder.Entity("eAchizitii.Models.InfoComanda", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("ComandaId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Data")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descriere")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ComandaId");

                    b.ToTable("InfoComanda");
                });

            modelBuilder.Entity("eAchizitii.Models.Produs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Denumire")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<int>("Um")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Produse");
                });

            modelBuilder.Entity("eAchizitii.Models.Produs_comanda", b =>
                {
                    b.Property<int>("ProdusId")
                        .HasColumnType("int");

                    b.Property<int?>("ComandaId")
                        .HasColumnType("int");

                    b.Property<int>("Cantitate")
                        .HasColumnType("int");

                    b.HasKey("ProdusId", "ComandaId");

                    b.HasIndex("ComandaId");

                    b.ToTable("Produse_comenzi");
                });

            modelBuilder.Entity("eAchizitii.Models.Sucursala", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Denumire")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Sucursale");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("eAchizitii.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("eAchizitii.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("eAchizitii.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("eAchizitii.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("eAchizitii.Models.AdresaLivrare", b =>
                {
                    b.HasOne("eAchizitii.Models.Sucursala", "sucursala")
                        .WithMany("AdreseLivrare")
                        .HasForeignKey("SucursalaId");

                    b.Navigation("sucursala");
                });

            modelBuilder.Entity("eAchizitii.Models.Angajat_comanda", b =>
                {
                    b.HasOne("eAchizitii.Models.Comanda", "comanda")
                        .WithMany("angajati_comenzi")
                        .HasForeignKey("ComandaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("eAchizitii.Models.AppUser", "angajat")
                        .WithMany("angajati_comenzi")
                        .HasForeignKey("angajatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("angajat");

                    b.Navigation("comanda");
                });

            modelBuilder.Entity("eAchizitii.Models.AppUser", b =>
                {
                    b.HasOne("eAchizitii.Models.Sucursala", "sucursala")
                        .WithMany("appUsers")
                        .HasForeignKey("SucursalaId");

                    b.Navigation("sucursala");
                });

            modelBuilder.Entity("eAchizitii.Models.Comanda", b =>
                {
                    b.HasOne("eAchizitii.Models.AdresaLivrare", "adresaLivrare")
                        .WithMany("comenzi")
                        .HasForeignKey("AdresaLivrareId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("eAchizitii.Models.Sucursala", "sucursala")
                        .WithMany("Comenzi")
                        .HasForeignKey("SucursalaId");

                    b.Navigation("adresaLivrare");

                    b.Navigation("sucursala");
                });

            modelBuilder.Entity("eAchizitii.Models.InfoComanda", b =>
                {
                    b.HasOne("eAchizitii.Models.Comanda", "comanda")
                        .WithMany("InfoComenzi")
                        .HasForeignKey("ComandaId");

                    b.Navigation("comanda");
                });

            modelBuilder.Entity("eAchizitii.Models.Produs_comanda", b =>
                {
                    b.HasOne("eAchizitii.Models.Comanda", "comanda")
                        .WithMany("produse_comenzi")
                        .HasForeignKey("ComandaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("eAchizitii.Models.Produs", "produs")
                        .WithMany("produse_comenzi")
                        .HasForeignKey("ProdusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("comanda");

                    b.Navigation("produs");
                });

            modelBuilder.Entity("eAchizitii.Models.AdresaLivrare", b =>
                {
                    b.Navigation("comenzi");
                });

            modelBuilder.Entity("eAchizitii.Models.AppUser", b =>
                {
                    b.Navigation("angajati_comenzi");
                });

            modelBuilder.Entity("eAchizitii.Models.Comanda", b =>
                {
                    b.Navigation("InfoComenzi");

                    b.Navigation("angajati_comenzi");

                    b.Navigation("produse_comenzi");
                });

            modelBuilder.Entity("eAchizitii.Models.Produs", b =>
                {
                    b.Navigation("produse_comenzi");
                });

            modelBuilder.Entity("eAchizitii.Models.Sucursala", b =>
                {
                    b.Navigation("AdreseLivrare");

                    b.Navigation("Comenzi");

                    b.Navigation("appUsers");
                });
#pragma warning restore 612, 618
        }
    }
}
