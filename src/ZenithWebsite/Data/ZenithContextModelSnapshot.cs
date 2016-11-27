using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using ZenithWebsite.Data;

namespace ZenithWebsite.Data
{
    [DbContext(typeof(ZenithContext))]
    partial class ZenithContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1");

            modelBuilder.Entity("ZenithWebsite.Models.ZenithSocietyModels.Activity", b =>
                {
                    b.Property<int>("ActivityId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ActivityDescription")
                        .IsRequired();

                    b.Property<DateTime>("CreationDate");

                    b.HasKey("ActivityId");

                    b.ToTable("Activity");
                });

            modelBuilder.Entity("ZenithWebsite.Models.ZenithSocietyModels.Event", b =>
                {
                    b.Property<int>("EventId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ActivityId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreationDate");

                    b.Property<DateTime>("FromDate");

                    b.Property<bool>("IsActive");

                    b.Property<DateTime>("ToDate");

                    b.HasKey("EventId");

                    b.HasIndex("ActivityId");

                    b.ToTable("Event");
                });

            modelBuilder.Entity("ZenithWebsite.Models.ZenithSocietyModels.Event", b =>
                {
                    b.HasOne("ZenithWebsite.Models.ZenithSocietyModels.Activity", "Activity")
                        .WithMany()
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
