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

            modelBuilder.Entity("Intranet.API.Domain.Models.Entities.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Client");
                });

            modelBuilder.Entity("Intranet.API.Domain.Models.Entities.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("City");

                    b.Property<string>("Description");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("Mobile");

                    b.Property<string>("PhoneNumber");

                    b.Property<int>("PostalCode");

                    b.Property<string>("StreetAdress");

                    b.HasKey("Id");

                    b.ToTable("Employee");
                });

            modelBuilder.Entity("Intranet.API.Domain.Models.Entities.EmployeeSkill", b =>
                {
                    b.Property<int>("EmployeeId");

                    b.Property<int>("SkillId");

                    b.Property<int>("CurrentLevel");

                    b.HasKey("EmployeeId", "SkillId");

                    b.HasIndex("CurrentLevel");

                    b.HasIndex("SkillId");

                    b.ToTable("EmployeeSkill");
                });

            modelBuilder.Entity("Intranet.API.Domain.Models.Entities.EmployeeToDo", b =>
                {
                    b.Property<int>("ToDoId");

                    b.Property<int>("EmployeeId");

                    b.Property<bool>("Done");

                    b.HasKey("ToDoId", "EmployeeId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("EmployeeToDo");
                });

            modelBuilder.Entity("Intranet.API.Domain.Models.Entities.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FileName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Image");
                });

            modelBuilder.Entity("Intranet.API.Domain.Models.Entities.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Coordinate");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Location");
                });

            modelBuilder.Entity("Intranet.API.Domain.Models.Entities.News", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author")
                        .IsRequired();

                    b.Property<DateTimeOffset>("Date");

                    b.Property<int?>("HeaderImageId");

                    b.Property<string>("Text")
                        .IsRequired();

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("HeaderImageId");

                    b.ToTable("News");
                });

            modelBuilder.Entity("Intranet.API.Domain.Models.Entities.NewsTag", b =>
                {
                    b.Property<int>("NewsId");

                    b.Property<int>("TagId");

                    b.HasKey("NewsId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("NewsTag");
                });

            modelBuilder.Entity("Intranet.API.Domain.Models.Entities.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ClientId");

                    b.Property<string>("Description");

                    b.Property<int>("LocationId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("LocationId");

                    b.ToTable("Project");
                });

            modelBuilder.Entity("Intranet.API.Domain.Models.Entities.ProjectEmployee", b =>
                {
                    b.Property<int>("EmployeeId");

                    b.Property<int>("ProjectId");

                    b.Property<bool>("Active");

                    b.Property<DateTimeOffset>("EndDate");

                    b.Property<string>("InformalDescription");

                    b.Property<int>("RoleId");

                    b.Property<DateTimeOffset>("StartDate");

                    b.HasKey("EmployeeId", "ProjectId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("RoleId");

                    b.ToTable("ProjectEmployee");
                });

            modelBuilder.Entity("Intranet.API.Domain.Models.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("Intranet.API.Domain.Models.Entities.Skill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Skill");
                });

            modelBuilder.Entity("Intranet.API.Domain.Models.Entities.SkillLevel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("SkillLevel");
                });

            modelBuilder.Entity("Intranet.API.Domain.Models.Entities.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Tag");
                });

            modelBuilder.Entity("Intranet.API.Domain.Models.Entities.ToDo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("ToDo");
                });

            modelBuilder.Entity("Intranet.API.Domain.Models.Entities.EmployeeSkill", b =>
                {
                    b.HasOne("Intranet.API.Domain.Models.Entities.SkillLevel", "Current")
                        .WithMany("CurrentSkillLevels")
                        .HasForeignKey("CurrentLevel")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Intranet.API.Domain.Models.Entities.Employee", "Employee")
                        .WithMany("EmployeeSkills")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Intranet.API.Domain.Models.Entities.Skill", "Skill")
                        .WithMany("EmployeeSkills")
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Intranet.API.Domain.Models.Entities.EmployeeToDo", b =>
                {
                    b.HasOne("Intranet.API.Domain.Models.Entities.Employee", "Employee")
                        .WithMany("EmployeeToDos")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Intranet.API.Domain.Models.Entities.ToDo", "ToDo")
                        .WithMany("EmployeeToDos")
                        .HasForeignKey("ToDoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Intranet.API.Domain.Models.Entities.News", b =>
                {
                    b.HasOne("Intranet.API.Domain.Models.Entities.Image", "HeaderImage")
                        .WithMany()
                        .HasForeignKey("HeaderImageId");
                });

            modelBuilder.Entity("Intranet.API.Domain.Models.Entities.NewsTag", b =>
                {
                    b.HasOne("Intranet.API.Domain.Models.Entities.News", "News")
                        .WithMany("NewsTags")
                        .HasForeignKey("NewsId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Intranet.API.Domain.Models.Entities.Tag", "Tag")
                        .WithMany("NewsTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Intranet.API.Domain.Models.Entities.Project", b =>
                {
                    b.HasOne("Intranet.API.Domain.Models.Entities.Client", "Client")
                        .WithMany("Projects")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Intranet.API.Domain.Models.Entities.Location", "Location")
                        .WithMany("Projects")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Intranet.API.Domain.Models.Entities.ProjectEmployee", b =>
                {
                    b.HasOne("Intranet.API.Domain.Models.Entities.Employee", "Employee")
                        .WithMany("ProjectEmployees")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Intranet.API.Domain.Models.Entities.Project", "Project")
                        .WithMany("ProjectEmployees")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Intranet.API.Domain.Models.Entities.Role", "Role")
                        .WithMany("ProjectEmployees")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
