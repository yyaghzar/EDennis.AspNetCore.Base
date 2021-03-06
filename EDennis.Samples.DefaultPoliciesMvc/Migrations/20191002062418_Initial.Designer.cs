﻿// <auto-generated />
using EDennis.Samples.DefaultPoliciesMvc.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EDennis.Samples.DefaultPoliciesMvc.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20191002062418_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("EDennis.Samples.DefaultPoliciesMvc.Models.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Person");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Moe"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Larry"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Curly"
                        });
                });

            modelBuilder.Entity("EDennis.Samples.DefaultPoliciesMvc.Models.Position", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("Position");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Title = "Manager"
                        },
                        new
                        {
                            Id = 2,
                            Title = "Employee"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
