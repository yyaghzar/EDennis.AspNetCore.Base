﻿// <auto-generated />
using System;
using EDennis.Samples.Hr.InternalApi1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EDennis.Samples.Hr.InternalApi1.Migrations.HrHistory
{
    [DbContext(typeof(HrHistoryContext))]
    [Migration("20190227201457_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EDennis.Samples.Hr.InternalApi1.Models.Employee", b =>
                {
                    b.Property<int>("Id");

                    b.Property<DateTime>("SysStart");

                    b.Property<string>("FirstName")
                        .HasMaxLength(30);

                    b.Property<DateTime>("SysEnd");

                    b.Property<string>("SysUser");

                    b.Property<string>("SysUserNext");

                    b.HasKey("Id", "SysStart");

                    b.ToTable("Employee","dbo_history");
                });

            modelBuilder.Entity("EDennis.Samples.Hr.InternalApi1.Models.EmployeePosition", b =>
                {
                    b.Property<int>("EmployeeId");

                    b.Property<int>("PositionId");

                    b.Property<DateTime>("SysStart");

                    b.Property<int?>("EmployeeId1");

                    b.Property<DateTime?>("EmployeeSysStart");

                    b.Property<int?>("PositionId1");

                    b.Property<DateTime?>("PositionSysStart");

                    b.Property<DateTime>("SysEnd");

                    b.Property<string>("SysUser");

                    b.Property<string>("SysUserNext");

                    b.HasKey("EmployeeId", "PositionId", "SysStart");

                    b.HasIndex("EmployeeId1", "EmployeeSysStart");

                    b.HasIndex("PositionId1", "PositionSysStart");

                    b.ToTable("EmployeePosition","dbo_history");
                });

            modelBuilder.Entity("EDennis.Samples.Hr.InternalApi1.Models.Position", b =>
                {
                    b.Property<int>("Id");

                    b.Property<DateTime>("SysStart");

                    b.Property<bool>("IsManager");

                    b.Property<DateTime>("SysEnd");

                    b.Property<string>("SysUser");

                    b.Property<string>("SysUserNext");

                    b.Property<string>("Title")
                        .HasMaxLength(60);

                    b.HasKey("Id", "SysStart");

                    b.ToTable("Position","dbo_history");
                });

            modelBuilder.Entity("EDennis.Samples.Hr.InternalApi1.Models.EmployeePosition", b =>
                {
                    b.HasOne("EDennis.Samples.Hr.InternalApi1.Models.Employee", "Employee")
                        .WithMany("EmployeePositions")
                        .HasForeignKey("EmployeeId1", "EmployeeSysStart");

                    b.HasOne("EDennis.Samples.Hr.InternalApi1.Models.Position", "Position")
                        .WithMany("EmployeePositions")
                        .HasForeignKey("PositionId1", "PositionSysStart");
                });
#pragma warning restore 612, 618
        }
    }
}
