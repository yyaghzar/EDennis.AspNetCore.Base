﻿// <auto-generated />
using System;
using EDennis.Samples.Hr.InternalApi2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EDennis.Samples.Hr.InternalApi2.Migrations.AgencyOnlineCheck
{
    [DbContext(typeof(AgencyOnlineCheckContext))]
    partial class AgencyOnlineCheckContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EDennis.Samples.Hr.InternalApi2.Models.AgencyOnlineCheck", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateCompleted")
                        .HasColumnType("date");

                    b.Property<int>("EmployeeId");

                    b.Property<string>("Status")
                        .HasMaxLength(100);

                    b.Property<string>("SysUser");

                    b.HasKey("Id");

                    b.ToTable("AgencyOnlineCheck");
                });
#pragma warning restore 612, 618
        }
    }
}
