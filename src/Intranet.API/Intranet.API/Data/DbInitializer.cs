using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Intranet.API.Domain.Data;
using Intranet.API.Domain.Models.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Intranet.API.Data
{
    public static class DbInitializer
    {
        /// Check if DB is empty and seed it.
        /// source: http://stackoverflow.com/questions/34536021/seed-initial-data-in-entity-framework-7-rc-1-and-asp-net-mvc-6
        public static void SeedDb(IntranetApiContext context)
        {
            context.Database.EnsureCreated();

            var news = new News[]
            {
                new News
                {
                    Date = new DateTimeOffset(new DateTime(2017, 04, 03)),
                    Title = "News title 1",
                    Text = "This is a content placeholder for news title 1.",
                    Author = "Anne Annesson"
                },
                new News
                {
                    Date = new DateTimeOffset(new DateTime(2017, 04, 02)),
                    Title = "News title    2",
                    Text = "This is a content placeholder for news title 2",
                    Author = "Anne Annesson"
                },
                new News
                {
                    Date = new DateTimeOffset(new DateTime(2017, 03, 31)),
                    Title = "News title    3",
                    Text = "This is a content placeholder for news title 3",
                    Author = "Anne Annesson"
                },
                new News
                {
                    Date = new DateTimeOffset(new DateTime(2017, 03, 30)),
                    Title = "News title    4",
                    Text = "This is a content placeholder for news title 4",
                    Author = "Anne Annesson"
                },
                new News
                {
                    Date = new DateTimeOffset(new DateTime(2017, 03, 29)),
                    Title = "News title    5",
                    Text = "This is a content placeholder for news title 5",
                    Author = "Anne Annesson"
                }
            };

            context.News.AddRange(news);
            context.SaveChanges();

            var toDoList = new ToDo[]
            {
                new ToDo
                {
                    Description = "Read document with new employee instructions."
                },
                new ToDo
                {
                    Description = "Obtain a mobile phone."
                },
                new ToDo
                {
                    Description = "Obtain a computer."
                },
                new ToDo
                {
                    Description = "Obtain an email address."
                },
                new ToDo
                {
                    Description = "Submit your bank account details for salary."
                }
            };

            context.ToDos.AddRange(toDoList);
            context.SaveChanges();

            var employees = new Employee[]
            {
                new Employee
                {
                    FirstName = "Connie",
                    LastName = "Conniesson",
                    Description = "Consultant, socially active and time sensitive.",
                    Email = "connie.conniesson@certaincy.com",
                    PhoneNumber = "1234-567890",
                    Mobile = "1234-567890",
                    StreetAdress = "Connie Conniessons Gata 1",
                    PostalCode = 12345,
                    City = "Gothenburg"
                },
                new Employee
                {
                    FirstName = "Nils",
                    LastName = "Nilsson",
                    Description = "Consultant, new on the job and getting to grips with how things work.",
                    Email = "nils.nilsson@certaincy.com",
                    PhoneNumber = "1234-567890",
                    Mobile = "1234-567890",
                    StreetAdress = "Nils Nilssons Gata 1",
                    PostalCode = 12345,
                    City = "Gothenburg"
                }
            };

            context.Employees.AddRange(employees);
            context.SaveChanges();

            var employeeToDoList = new EmployeeToDo[]
            {
                    new EmployeeToDo
                    {
                        EmployeeId = 1,
                        ToDoId = 1,
                        Done = false
                    },
                    new EmployeeToDo
                    {
                        EmployeeId = 1,
                        ToDoId = 2,
                        Done = true
                    },
                    new EmployeeToDo
                    {
                        EmployeeId = 1,
                        ToDoId = 3,
                        Done = false
                    },
                    new EmployeeToDo
                    {
                        EmployeeId = 1,
                        ToDoId = 4,
                        Done = false
                    },
                    new EmployeeToDo
                    {
                        EmployeeId = 1,
                        ToDoId = 5,
                        Done = false
                    },
                    new EmployeeToDo
                    {
                        EmployeeId = 2,
                        ToDoId = 1,
                        Done = true
                    },
                    new EmployeeToDo
                    {
                        EmployeeId = 2,
                        ToDoId = 2,
                        Done = true
                    },
                    new EmployeeToDo
                    {
                        EmployeeId = 2,
                        ToDoId = 3,
                        Done = true
                    },
                    new EmployeeToDo
                    {
                        EmployeeId = 2,
                        ToDoId = 4,
                        Done = true
                    },
                    new EmployeeToDo
                    {
                        EmployeeId = 2,
                        ToDoId = 5,
                        Done = true
                    }
            };

            context.EmployeeToDos.AddRange(employeeToDoList);
            context.SaveChanges();

            var skills = new Skill[]
            {
                new Skill
                {
                    Description = "C#"
                },
                new Skill
                {
                    Description = "C++"
                },
                new Skill
                {
                    Description = "JavaScript"
                },
                new Skill
                {
                    Description = "Java"
                },
                new Skill
                {
                    Description = "Python"
                },
                new Skill
                {
                    Description = "Linux"
                },
                new Skill
                {
                    Description = "Unix"
                }
            };

            context.Skills.AddRange(skills);
            context.SaveChanges();

            var skillLevels = new SkillLevel[]
            {
                new SkillLevel
                {
                    Description = "Beginner"
                },
                new SkillLevel
                {
                    Description = "Intermediate"
                },
                new SkillLevel
                {
                    Description = "Advanced"
                },
                new SkillLevel
                {
                    Description = "Expert"
                }
            };

            context.SkillLevels.AddRange(skillLevels);
            context.SaveChanges();

            var employeeSkills = new EmployeeSkill[]
            {
                new EmployeeSkill
                {
                    EmployeeId = 1,                 // Employee: Connie
                    SkillId = 1,                    // Skill: C#
                    CurrentLevel = 4,               // Current level: Expert
                    DesiredLevel = 4
                },
                new EmployeeSkill
                {
                    EmployeeId = 1,
                    SkillId = 4,                    // Skill: Java
                    CurrentLevel = 3,               // Current level: Advanced
                    DesiredLevel = 4                // Desired level: Expert
                },
                new EmployeeSkill
                {
                    EmployeeId = 2,                 // Employee: Nils
                    SkillId = 1,                    // Skill: C#
                    CurrentLevel = 1,               // Current level: Beginner
                    DesiredLevel = 3                // Desired level: Advanced
                },
                new EmployeeSkill
                {
                    EmployeeId = 2,
                    SkillId = 3,                    // Skill: JavaScript
                    CurrentLevel = 2,               // Current level: Intermediate
                    DesiredLevel = 4
                },
                new EmployeeSkill
                {
                    EmployeeId = 2,
                    SkillId = 5,                    // Skill: Python
                    CurrentLevel = 1,
                    DesiredLevel = 2
                }
            };

            context.EmployeeSkills.AddRange(employeeSkills);
            context.SaveChanges();

            var clients = new Client[]
            {
                new Client
                {
                    Name = "Volvo Trucks",
                    Description = "Automotive company, manufacturer of heavy duty and light weight trucks."
                },
                new Client
                {
                    Name = "Volvo Cars",
                    Description = "Automotive company, manufacturer of cars."
                },
                new Client
                {
                    Name = "SCA",
                    Description = "Hygiene and forest products company."
                }
            };

            context.Clients.AddRange(clients);
            context.SaveChanges();

            var projects = new Project[]
            {
                new Project
                {
                    Name = "HMMIOM digital dashboard / GTT",
                    Description = "Migration from mechanical to fully digital driver dashboard.",
                    ClientId = 1
                },
                new Project
                {
                    Name = "Web page development",
                    Description = "Development of new customer portal.",
                    ClientId = 3
                }
            };

            context.Projects.AddRange(projects);
            context.SaveChanges();

            var locations = new Location[]
            {
                new Location
                {
                    Description = "GTT"
                },
                new Location
                {
                    Description = "Certaincy"
                }
            };

            context.Locations.AddRange(locations);
            context.SaveChanges();

            var roles = new Role[]
            {
                new Role
                {
                    Description = "Component owner"
                },
                new Role
                {
                    Description = "Tester"
                }
            };

            context.Roles.AddRange(roles);
            context.SaveChanges();

            var assignments = new ProjectEmployee[]
            {
                new ProjectEmployee
                {
                    EmployeeId = 1,
                    ProjectId = 1,
                    LocationId = 1,
                    Active = true,
                    StartDate = DateTimeOffset.UtcNow,
                    InformalDescription = "Component owner for HMIIOM ECU. X-functional responsibility between project, manufacturing and after market level to ensure successful deliveries and long term sustainability."
                },
                new ProjectEmployee
                {
                    EmployeeId = 2,
                    ProjectId = 2,
                    LocationId = 2,
                    Active = true,
                    StartDate = DateTimeOffset.UtcNow,
                    InformalDescription = "Test developer for SCA web portal."
                }
            };

            context.ProjectEmployees.AddRange(assignments);
            context.SaveChanges();
        }
    }
}
