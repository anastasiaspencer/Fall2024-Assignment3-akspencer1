﻿// <auto-generated />
using Fall2024_Assignment3_akspencer1.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Fall2024_Assignment3_akspencer1.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241027224533_UpdateDatabaseAfterManualChanges")]
    partial class UpdateDatabaseAfterManualChanges
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ActorMovie", b =>
                {
                    b.Property<string>("ActorsName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MoviesTitle")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ActorsName", "MoviesTitle");

                    b.HasIndex("MoviesTitle");

                    b.ToTable("MovieActors", (string)null);
                });

            modelBuilder.Entity("Fall2024_Assignment3_akspencer1.Models.Actor", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImdbLink")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhotoUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Name");

                    b.ToTable("Actors");
                });

            modelBuilder.Entity("Fall2024_Assignment3_akspencer1.Models.Movie", b =>
                {
                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Genre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImdbLink")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PosterUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("YearOfRelease")
                        .HasColumnType("int");

                    b.HasKey("Title");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("ActorMovie", b =>
                {
                    b.HasOne("Fall2024_Assignment3_akspencer1.Models.Actor", null)
                        .WithMany()
                        .HasForeignKey("ActorsName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Fall2024_Assignment3_akspencer1.Models.Movie", null)
                        .WithMany()
                        .HasForeignKey("MoviesTitle")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
