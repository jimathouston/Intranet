using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Intranet.Web.Domain.Data;

namespace Intranet.Web.Domain.Migrations
{
    [DbContext(typeof(IntranetApiContext))]
    [Migration("20170728145548_Policy_Migration")]
    partial class Policy_Migration
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

            modelBuilder.Entity("Intranet.Web.Domain.Models.Entities.FaqKeyword", b =>
                {
                    b.Property<int>("FaqId");

                    b.Property<int>("KeywordId");

                    b.HasKey("FaqId", "KeywordId");

                    b.HasIndex("KeywordId");

                    b.ToTable("FaqKeyword");
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

            modelBuilder.Entity("Intranet.Web.Domain.Models.Entities.Keyword", b =>
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

                    b.ToTable("Keyword");
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

            modelBuilder.Entity("Intranet.Web.Domain.Models.Entities.NewsKeyword", b =>
                {
                    b.Property<int>("NewsId");

                    b.Property<int>("KeywordId");

                    b.HasKey("NewsId", "KeywordId");

                    b.HasIndex("KeywordId");

                    b.ToTable("NewsKeyword");
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

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("Url")
                        .IsUnique();

                    b.ToTable("Policy");
                });

            modelBuilder.Entity("Intranet.Web.Domain.Models.Entities.PolicyKeyword", b =>
                {
                    b.Property<int>("PolicyId");

                    b.Property<int>("KeywordId");

                    b.HasKey("PolicyId", "KeywordId");

                    b.HasIndex("KeywordId");

                    b.ToTable("PolicyKeyword");
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

            modelBuilder.Entity("Intranet.Web.Domain.Models.Entities.FaqKeyword", b =>
                {
                    b.HasOne("Intranet.Web.Domain.Models.Entities.Faq", "Faq")
                        .WithMany("FaqKeywords")
                        .HasForeignKey("FaqId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Intranet.Web.Domain.Models.Entities.Keyword", "Keyword")
                        .WithMany("FaqKeywords")
                        .HasForeignKey("KeywordId")
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

            modelBuilder.Entity("Intranet.Web.Domain.Models.Entities.NewsKeyword", b =>
                {
                    b.HasOne("Intranet.Web.Domain.Models.Entities.Keyword", "Keyword")
                        .WithMany("NewsKeywords")
                        .HasForeignKey("KeywordId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Intranet.Web.Domain.Models.Entities.News", "News")
                        .WithMany("NewsKeywords")
                        .HasForeignKey("NewsId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Intranet.Web.Domain.Models.Entities.Policy", b =>
                {
                    b.HasOne("Intranet.Web.Domain.Models.Entities.Category", "Category")
                        .WithMany("Policies")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Intranet.Web.Domain.Models.Entities.PolicyKeyword", b =>
                {
                    b.HasOne("Intranet.Web.Domain.Models.Entities.Keyword", "Keyword")
                        .WithMany("PolicyKeywords")
                        .HasForeignKey("KeywordId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Intranet.Web.Domain.Models.Entities.Policy", "Policy")
                        .WithMany("PolicyKeywords")
                        .HasForeignKey("PolicyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
