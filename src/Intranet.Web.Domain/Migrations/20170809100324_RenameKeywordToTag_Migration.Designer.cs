using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Intranet.Web.Domain.Data;

namespace Intranet.Web.Domain.Migrations
{
    [DbContext(typeof(IntranetApiContext))]
    [Migration("20170809100324_RenameKeywordToTag_Migration")]
    partial class RenameKeywordToTag_Migration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Intranet.Web.Domain.Models.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.HasIndex("Title")
                        .IsUnique();

                    b.HasIndex("Url")
                        .IsUnique();

                    b.ToTable("Category");
                });

            modelBuilder.Entity("Intranet.Web.Domain.Models.Entities.Faq", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Answer")
                        .IsRequired();

                    b.Property<int>("CategoryId");

                    b.Property<string>("Question")
                        .IsRequired();

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("Url")
                        .IsUnique();

                    b.ToTable("Faq");
                });

            modelBuilder.Entity("Intranet.Web.Domain.Models.Entities.FaqTag", b =>
                {
                    b.Property<int>("FaqId");

                    b.Property<int>("TagId");

                    b.HasKey("FaqId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("FaqTag");
                });

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

                    b.Property<string>("Url")
                        .IsRequired();

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("HeaderImageId");

                    b.HasIndex("Url");

                    b.HasIndex("UserId");

                    b.ToTable("News");
                });

            modelBuilder.Entity("Intranet.Web.Domain.Models.Entities.NewsTag", b =>
                {
                    b.Property<int>("NewsId");

                    b.Property<int>("TagId");

                    b.HasKey("NewsId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("NewsTag");
                });

            modelBuilder.Entity("Intranet.Web.Domain.Models.Entities.Policy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CategoryId");

                    b.Property<string>("Description");

                    b.Property<string>("FileUrl");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<string>("Url")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("Url")
                        .IsUnique();

                    b.ToTable("Policy");
                });

            modelBuilder.Entity("Intranet.Web.Domain.Models.Entities.PolicyTag", b =>
                {
                    b.Property<int>("PolicyId");

                    b.Property<int>("TagId");

                    b.HasKey("PolicyId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("PolicyTag");
                });

            modelBuilder.Entity("Intranet.Web.Domain.Models.Entities.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("Url")
                        .IsUnique();

                    b.ToTable("Tag");
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

            modelBuilder.Entity("Intranet.Web.Domain.Models.Entities.Faq", b =>
                {
                    b.HasOne("Intranet.Web.Domain.Models.Entities.Category", "Category")
                        .WithMany("Faqs")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Intranet.Web.Domain.Models.Entities.FaqTag", b =>
                {
                    b.HasOne("Intranet.Web.Domain.Models.Entities.Faq", "Faq")
                        .WithMany("FaqTags")
                        .HasForeignKey("FaqId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Intranet.Web.Domain.Models.Entities.Tag", "Tag")
                        .WithMany("FaqTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
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

            modelBuilder.Entity("Intranet.Web.Domain.Models.Entities.NewsTag", b =>
                {
                    b.HasOne("Intranet.Web.Domain.Models.Entities.News", "News")
                        .WithMany("NewsTags")
                        .HasForeignKey("NewsId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Intranet.Web.Domain.Models.Entities.Tag", "Tag")
                        .WithMany("NewsTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Intranet.Web.Domain.Models.Entities.Policy", b =>
                {
                    b.HasOne("Intranet.Web.Domain.Models.Entities.Category", "Category")
                        .WithMany("Policies")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Intranet.Web.Domain.Models.Entities.PolicyTag", b =>
                {
                    b.HasOne("Intranet.Web.Domain.Models.Entities.Policy", "Policy")
                        .WithMany("PolicyTags")
                        .HasForeignKey("PolicyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Intranet.Web.Domain.Models.Entities.Tag", "Tag")
                        .WithMany("PolicyTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
