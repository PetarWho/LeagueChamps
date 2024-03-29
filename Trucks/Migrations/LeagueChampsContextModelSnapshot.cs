﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Trucks.Data;

#nullable disable

namespace LeagueChampsDb.Migrations
{
    [DbContext(typeof(LeagueChampsContext))]
    partial class LeagueChampsContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("LeagueChampsDb.Data.Models.Ability", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("ChampionId")
                        .HasColumnType("int");

                    b.Property<string>("ImageURL")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsPassive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ChampionId");

                    b.ToTable("Abilities");
                });

            modelBuilder.Entity("LeagueChampsDb.Data.Models.Champion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Class")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("HasMana")
                        .HasColumnType("bit");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsRanged")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Position")
                        .HasColumnType("int");

                    b.Property<int>("PriceBE")
                        .HasColumnType("int");

                    b.Property<int>("PriceRP")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Champions");
                });

            modelBuilder.Entity("LeagueChampsDb.Data.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("LeagueChampsDb.Data.Models.UserChampion", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("ChampionId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "ChampionId");

                    b.HasIndex("ChampionId");

                    b.ToTable("UsersChampions");
                });

            modelBuilder.Entity("LeagueChampsDb.Data.Models.Ability", b =>
                {
                    b.HasOne("LeagueChampsDb.Data.Models.Champion", null)
                        .WithMany("Abilities")
                        .HasForeignKey("ChampionId");
                });

            modelBuilder.Entity("LeagueChampsDb.Data.Models.UserChampion", b =>
                {
                    b.HasOne("LeagueChampsDb.Data.Models.Champion", "Champion")
                        .WithMany("UsersChampions")
                        .HasForeignKey("ChampionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LeagueChampsDb.Data.Models.User", "User")
                        .WithMany("UsersChampions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Champion");

                    b.Navigation("User");
                });

            modelBuilder.Entity("LeagueChampsDb.Data.Models.Champion", b =>
                {
                    b.Navigation("Abilities");

                    b.Navigation("UsersChampions");
                });

            modelBuilder.Entity("LeagueChampsDb.Data.Models.User", b =>
                {
                    b.Navigation("UsersChampions");
                });
#pragma warning restore 612, 618
        }
    }
}
