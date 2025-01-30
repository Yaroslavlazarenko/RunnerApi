﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Persistence.Contexts;

#nullable disable

namespace Persistence.Migrations
{
    [DbContext(typeof(RunnersContext))]
    [Migration("20250122114324_EditRunnerTable")]
    partial class EditRunnerTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Core.Entities.Race", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("RaceGender")
                        .HasColumnType("integer");

                    b.Property<int>("RaceLength")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Races");
                });

            modelBuilder.Entity("Core.Entities.RaceStatistic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("RaceId")
                        .HasColumnType("integer");

                    b.Property<int>("RunnerId")
                        .HasColumnType("integer");

                    b.Property<TimeSpan>("TimeResult")
                        .HasColumnType("interval");

                    b.HasKey("Id");

                    b.HasIndex("RaceId");

                    b.HasIndex("RunnerId");

                    b.ToTable("RaceStatistics");
                });

            modelBuilder.Entity("Core.Entities.Runner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Gender")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("RatingValue")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Runners");
                });

            modelBuilder.Entity("Core.Entities.RaceStatistic", b =>
                {
                    b.HasOne("Core.Entities.Race", "Race")
                        .WithMany("Statistics")
                        .HasForeignKey("RaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Entities.Runner", "Runner")
                        .WithMany("Statistics")
                        .HasForeignKey("RunnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Race");

                    b.Navigation("Runner");
                });

            modelBuilder.Entity("Core.Entities.Race", b =>
                {
                    b.Navigation("Statistics");
                });

            modelBuilder.Entity("Core.Entities.Runner", b =>
                {
                    b.Navigation("Statistics");
                });
#pragma warning restore 612, 618
        }
    }
}
