using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Intranet.Web.Domain.Data;

namespace Intranet.Web.Domain.Migrations
{
    [DbContext(typeof(IntranetApiContext))]
    [Migration("20170718122651_NewsPublish")]
    partial class NewsPublish
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Intranet.Web.Domain.Models.Entities.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FileName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Image");
                });

            modelBuilder.Entity("Intranet.Web.Domain.Models.Entities.News", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("Created");

                    b.Property<bool>("HasEverBeenPublished")
                        .HasAnnotation("PropertyAccessMode", PropertyAccessMode.FieldDuringConstruction);

                    b.Property<int?>("HeaderImageId");

                    b.Property<bool>("Published")
                        .HasAnnotation("PropertyAccessMode", PropertyAccessMode.Property);

                    b.Property<string>("Text")
                        .IsRequired();

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<DateTimeOffset>("Updated");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("HeaderImageId");

                    b.HasIndex("UserId");

                    b.ToTable("News");
                });

            modelBuilder.Entity("Intranet.Web.Domain.Models.Entities.User", b =>
                {
                    b.Property<string>("Username")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DisplayName")
                        .IsRequired();

                    b.HasKey("Username");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Intranet.Web.Domain.Models.Entities.News", b =>
                {
                    b.HasOne("Intranet.Web.Domain.Models.Entities.Image", "HeaderImage")
                        .WithMany()
                        .HasForeignKey("HeaderImageId");

                    b.HasOne("Intranet.Web.Domain.Models.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
