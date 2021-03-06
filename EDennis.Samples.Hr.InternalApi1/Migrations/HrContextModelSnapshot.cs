﻿// <auto-generated />
using System;
using EDennis.Samples.Hr.InternalApi1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EDennis.Samples.Hr.InternalApi1.Migrations
{
    [DbContext(typeof(HrContext))]
    partial class HrContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EDennis.Samples.Hr.InternalApi1.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FirstName")
                        .HasMaxLength(30);

                    b.Property<DateTime>("SysEnd");

                    b.Property<DateTime>("SysStart");

                    b.Property<string>("SysUser");

                    b.Property<string>("SysUserNext");

                    b.HasKey("Id");

                    b.ToTable("Employee","dbo");
                });

            modelBuilder.Entity("EDennis.Samples.Hr.InternalApi1.Models.EmployeePosition", b =>
                {
                    b.Property<int>("EmployeeId");

                    b.Property<int>("PositionId");

                    b.Property<DateTime>("SysEnd");

                    b.Property<DateTime>("SysStart");

                    b.Property<string>("SysUser");

                    b.Property<string>("SysUserNext");

                    b.HasKey("EmployeeId", "PositionId");

                    b.HasIndex("PositionId");

                    b.ToTable("EmployeePosition","dbo");
                });

            modelBuilder.Entity("EDennis.Samples.Hr.InternalApi1.Models.Position", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsManager");

                    b.Property<DateTime>("SysEnd");

                    b.Property<DateTime>("SysStart");

                    b.Property<string>("SysUser");

                    b.Property<string>("SysUserNext");

                    b.Property<string>("Title")
                        .HasMaxLength(60);

                    b.HasKey("Id");

                    b.ToTable("Position","dbo");
                });

            modelBuilder.Entity("EDennis.Samples.Hr.InternalApi1.Models.EmployeePosition", b =>
                {
                    b.HasOne("EDennis.Samples.Hr.InternalApi1.Models.Employee", "Employee")
                        .WithMany("EmployeePositions")
                        .HasForeignKey("EmployeeId")
                        .HasConstraintName("fk_EmployeePosition_Employee")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("EDennis.Samples.Hr.InternalApi1.Models.Position", "Position")
                        .WithMany("EmployeePositions")
                        .HasForeignKey("PositionId")
                        .HasConstraintName("fk_EmployeePosition_Position")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
