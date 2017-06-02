using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Intranet.API.Controllers;
using Intranet.API.Domain.Data;
using Intranet.API.Domain.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Intranet.API.UnitTests.Fakes;
using Xunit;

namespace Intranet.API.UnitTests.Controllers
{
    public class EmployeeSkillController_Fact
    {
        [Fact]
        public void ReturnOkWhenGetAllEmployeeSkills()
        {
            // Assign
            int id = 1;
            var employee = GetStubEmployee();
            var skills = GetStubSkills();
            var skillLevels = GetStubSkillLevels();
            var employeeSkills = GetStubEmployeeSkills();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Employees.Add(employee), ensureDeleted: true);
            DbContextFake.SeedDb<IntranetApiContext>(c => c.Skills.AddRange(skills), ensureDeleted: false);
            DbContextFake.SeedDb<IntranetApiContext>(c => c.SkillLevels.AddRange(skillLevels), ensureDeleted: false);
            DbContextFake.SeedDb<IntranetApiContext>(c => c.EmployeeSkills.AddRange(employeeSkills), ensureDeleted: false);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var controller = new EmployeeSkillController(context);

                // Act
                var result = controller.Get(id);

                // Assert
                Assert.IsType<OkObjectResult>(result);
            };
        }

        [Fact]
        public void ReturnNotFoundWhenGetAllEmpoyeeSKills()
        {
            // Assign
            int id = 1;

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var controller = new EmployeeSkillController(context);

                // Act
                var result = controller.Get(id);

                // Assert
                var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
                Assert.IsType<JsonResult>(notFoundResult.Value);
            };
        }

        [Fact]
        public void ReturnOkWhenGetSingleEmployeeSKill()
        {
            // Assign
            int id = 1;
            int skillId = 4;
            var employee = GetStubEmployee();
            var skills = GetStubSkills();
            var skillLevels = GetStubSkillLevels();
            var employeeSkills = GetStubEmployeeSkills();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Employees.Add(employee), ensureDeleted: true);
            DbContextFake.SeedDb<IntranetApiContext>(c => c.Skills.AddRange(skills), ensureDeleted: false);
            DbContextFake.SeedDb<IntranetApiContext>(c => c.SkillLevels.AddRange(skillLevels), ensureDeleted: false);
            DbContextFake.SeedDb<IntranetApiContext>(c => c.EmployeeSkills.AddRange(employeeSkills), ensureDeleted: false);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var controller = new EmployeeSkillController(context);

                // Act
                var result = controller.Get(id, skillId);

                // Assert
                var jSonResult = Assert.IsType<NotFoundObjectResult>(result);
                Assert.IsType<JsonResult>(jSonResult.Value);
            };
        }

        [Fact]
        public void ReturnNotFoundWhenGetSingleEmployeeSkill()
        {
            // Assign
            int id = 1;
            int skillId = 5;

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var controller = new EmployeeSkillController(context);

                // Act
                var result = controller.Get(id, skillId);

                // Assert
                var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
                Assert.IsType<JsonResult>(notFoundResult.Value);
            };
        }

        [Fact]
        public void ReturnOkWhenPostEmployeeSkill()
        {
            // Assign
            int id = 1;
            var employee = GetStubEmployee();
            var skills = GetStubSkills();
            var skillLevels = GetStubSkillLevels();
            var employeeSkill = GetStubEmployeeSkills().First();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Employees.Add(employee), ensureDeleted: true);
            DbContextFake.SeedDb<IntranetApiContext>(c => c.Skills.AddRange(skills), ensureDeleted: false);
            DbContextFake.SeedDb<IntranetApiContext>(c => c.SkillLevels.AddRange(skillLevels), ensureDeleted: false);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var controller = new EmployeeSkillController(context);

                // Act
                var result = controller.Post(id, employeeSkill);

                // Assert
                Assert.IsType<OkObjectResult>(result);
            };
        }

        [Fact]
        public void ReturnBadRequestWhenPostEmployeeSkill()
        {
            // Assign
            int id = 1;
            var skills = GetStubSkills();
            var skillLevels = GetStubSkillLevels();
            var employeeSkill = GetStubEmployeeSkills().First();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Skills.AddRange(skills), ensureDeleted: true);
            DbContextFake.SeedDb<IntranetApiContext>(c => c.SkillLevels.AddRange(skillLevels), ensureDeleted: false);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var controller = new EmployeeSkillController(context);

                // Act
                controller.ModelState.AddModelError("CurrentLevel", "Current skill level is required");
                var result = controller.Post(id, employeeSkill);

                // Assert
                var badReqResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.IsType<SerializableError>(badReqResult.Value);
            };
        }

        [Fact]
        public void ReturnOkWhenUpdateEmployeeSkill()
        {
            // Assign
            int id = 1;
            int skillId = 1;
            var employee = GetStubEmployee();
            var skills = GetStubSkills();
            var skillLevels = GetStubSkillLevels();
            var oldEmployeeSkill = GetStubEmployeeSkills().First();
            var newEmployeeSkill = GetStubEmployeeSkills().First();
            newEmployeeSkill.DesiredLevel = 4;

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Employees.Add(employee), ensureDeleted: true);
            DbContextFake.SeedDb<IntranetApiContext>(c => c.Skills.AddRange(skills), ensureDeleted: false);
            DbContextFake.SeedDb<IntranetApiContext>(c => c.SkillLevels.AddRange(skillLevels), ensureDeleted: false);
            DbContextFake.SeedDb<IntranetApiContext>(c => c.EmployeeSkills.Add(oldEmployeeSkill), ensureDeleted: false);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var controller = new EmployeeSkillController(context);

                // Act
                var result = controller.Put(id, skillId, newEmployeeSkill);

                // Assert
                Assert.IsType<OkObjectResult>(result);
            };
        }

        [Fact]
        public void ReturnNotFoundWhenUpdateEmployeeSkill()
        {
            // Assign
            int id = 1;
            int skillId = 1;
            var skills = GetStubSkills();
            var skillLevels = GetStubSkillLevels();
            var newEmployeeSkill = GetStubEmployeeSkills().First();
            newEmployeeSkill.DesiredLevel = 4;

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Skills.AddRange(skills), ensureDeleted: true);
            DbContextFake.SeedDb<IntranetApiContext>(c => c.SkillLevels.AddRange(skillLevels), ensureDeleted: false);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var controller = new EmployeeSkillController(context);

                // Act
                var result = controller.Put(id, skillId, newEmployeeSkill);

                // Assert
                var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
                Assert.IsType<JsonResult>(notFoundResult.Value);
            };
        }

        [Fact]
        public void ReturnBadRequestWhenUpdateEmployeeSkill()
        {
            // Assign
            int id = 1;
            int skillId = 1;
            var employee = GetStubEmployee();
            var skills = GetStubSkills();
            var skillLevels = GetStubSkillLevels();
            var oldEmployeeSkill = GetStubEmployeeSkills().First();
            var newEmployeeSkill = GetStubEmployeeSkills().First();
            newEmployeeSkill.DesiredLevel = 4;

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Employees.Add(employee), ensureDeleted: true);
            DbContextFake.SeedDb<IntranetApiContext>(c => c.Skills.AddRange(skills), ensureDeleted: false);
            DbContextFake.SeedDb<IntranetApiContext>(c => c.SkillLevels.AddRange(skillLevels), ensureDeleted: false);
            DbContextFake.SeedDb<IntranetApiContext>(c => c.EmployeeSkills.Add(oldEmployeeSkill), ensureDeleted: false);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var controller = new EmployeeSkillController(context);

                // Act
                controller.ModelState.AddModelError("CurrentLevel", "Current skill level must be provided.");
                var result = controller.Put(id, skillId, newEmployeeSkill);

                // Assert
                var badReqResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.IsType<SerializableError>(badReqResult.Value);
            };
        }

        [Fact]
        public void ReturnNotFoundWhenDeleteEmployeeSkill()
        {
            // Assign
            int id = 1;
            int skillId = 1;

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var controller = new EmployeeSkillController(context);

                // Act
                var result = controller.Delete(id, skillId);

                // Assert
                var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
                Assert.IsType<JsonResult>(notFoundResult.Value);
            };
        }

        [Fact]
        public void ReturnOkWhenDeleteEmployeeSkill()
        {
            // Assign
            int id = 1;
            int skillId = 1;
            var employeeSkill = GetStubEmployeeSkills().First();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.EmployeeSkills.Add(employeeSkill), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var controller = new EmployeeSkillController(context);

                // Act
                var result = controller.Delete(id, skillId);

                // Assert
                Assert.IsType<OkObjectResult>(result);
            }
        }

        private Employee GetStubEmployee()
        {
            return new Employee
            {
                Id = 1,
                FirstName = "Connie",
                LastName = "Conniesson",
                Description = "Consultant, socially active and time sensitive.",
                Email = "connie.conniesson@certaincy.com",
                PhoneNumber = "1234-567890",
                Mobile = "1234-567890",
                StreetAdress = "Connie Conniessons Gata 1",
                PostalCode = 12345,
                City = "Gothenburg"
            };
        }

        private IEnumerable<SkillLevel> GetStubSkillLevels()
        {
            return new SkillLevel[]
            {
        new SkillLevel
        {
          Id = 1,
          Description = "Beginner"
        },
        new SkillLevel
        {
          Id = 2,
          Description = "Intermediate"
        },
        new SkillLevel
        {
          Id = 3,
          Description = "Advanced"
        },
        new SkillLevel
        {
          Id = 4,
          Description = "Expert"
        }
            };
        }

        private IEnumerable<Skill> GetStubSkills()
        {
            return new Skill[]
            {
        new Skill
        {
          Id = 1,
          Description = "C#"
        },
        new Skill
        {
          Id = 2,
          Description = "C++"
        },
        new Skill
        {
          Id = 3,
          Description = "JavaScript"
        },
        new Skill
        {
          Id = 4,
          Description = "Java"
        },
        new Skill
        {
          Id = 5,
          Description = "Python"
        },
        new Skill
        {
          Id = 6,
          Description = "Linux"
        },
        new Skill
        {
          Id = 7,
          Description = "Unix"
        }
            };
        }

        private IEnumerable<EmployeeSkill> GetStubEmployeeSkills()
        {
            return new EmployeeSkill[]
            {
        new EmployeeSkill
        {
          EmployeeId = 1,
          SkillId = 1,
          CurrentLevel = 1,
          DesiredLevel = 2
        },
        new EmployeeSkill
        {
          EmployeeId = 1,
          SkillId = 2,
          CurrentLevel = 2,
          DesiredLevel = 4
        },
        new EmployeeSkill
        {
          EmployeeId = 1,
          SkillId = 3,
          CurrentLevel = 1,
          DesiredLevel = 2
        }
            };
        }
    }
}
