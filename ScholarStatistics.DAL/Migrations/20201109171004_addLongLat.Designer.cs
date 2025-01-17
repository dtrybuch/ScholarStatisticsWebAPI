﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ScholarStatistics.DAL;

namespace ScholarStatistics.DAL.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20201109171004_addLongLat")]
    partial class addLongLat
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("ScholarStatistics.DAL.Models.Affiliation", b =>
                {
                    b.Property<int>("AffiliationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("City")
                        .HasColumnType("text");

                    b.Property<string>("Country")
                        .HasColumnType("text");

                    b.Property<double>("Lattitude")
                        .HasColumnType("double precision");

                    b.Property<double>("Longitude")
                        .HasColumnType("double precision");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("AffiliationId");

                    b.ToTable("Affiliations");
                });

            modelBuilder.Entity("ScholarStatistics.DAL.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<double>("DifferenceBetweenPublicationsInDays")
                        .HasColumnType("double precision");

                    b.Property<string>("MainCountry")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<float>("RatioPublications")
                        .HasColumnType("real");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("ScholarStatistics.DAL.Models.Publication", b =>
                {
                    b.Property<int>("PublicationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("AffiliationFK")
                        .HasColumnType("integer");

                    b.Property<List<int>>("CategoriesFK")
                        .HasColumnType("integer[]");

                    b.Property<int>("CountOfCited")
                        .HasColumnType("integer");

                    b.Property<DateTime>("DateOfAddedToArxiv")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("DateOfPublished")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<bool>("WasCheckedInScopus")
                        .HasColumnType("boolean");

                    b.HasKey("PublicationID");

                    b.ToTable("Publications");
                });
#pragma warning restore 612, 618
        }
    }
}
