using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Intranet.API.Domain.Data;

namespace Intranet.API.Domain.Migrations
{
    [DbContext(typeof(IntranetApiContext))]
    partial class IntranetApiContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Intranet.API.Domain.Models.Entities.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FileName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Image");
                });

            modelBuilder.Entity("Intranet.API.Domain.Models.Entities.Keyword", b =>
                {
                    b.Property<string>("Name")
                        .ValueGeneratedOnAdd();

                    b.HasKey("Name");

                    b.ToTable("Keyword");
                });

            modelBuilder.Entity("Intranet.API.Domain.Models.Entities.News", b =>
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

            modelBuilder.Entity("Intranet.API.Domain.Models.Entities.NewsKeyword", b =>
                {
                    b.Property<int>("NewsId");

                    b.Property<string>("KeywordId");

                    b.HasKey("NewsId", "KeywordId");

                    b.HasIndex("KeywordId");

                    b.ToTable("NewsKeyword");
                });

            modelBuilder.Entity("Intranet.API.Domain.Models.Entities.User", b =>
                {
                    b.Property<string>("Username")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DisplayName")
                        .IsRequired();

                    b.HasKey("Username");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Intranet.API.Domain.Models.Entities.News", b =>
                {
                    b.HasOne("Intranet.API.Domain.Models.Entities.Image", "HeaderImage")
                        .WithMany()
                        .HasForeignKey("HeaderImageId");

                    b.HasOne("Intranet.API.Domain.Models.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Intranet.API.Domain.Models.Entities.NewsKeyword", b =>
                {
                    b.HasOne("Intranet.API.Domain.Models.Entities.Keyword", "Keyword")
                        .WithMany("NewsKeyword")
                        .HasForeignKey("KeywordId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Intranet.API.Domain.Models.Entities.News", "News")
                        .WithMany("NewsKeywords")
                        .HasForeignKey("NewsId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
