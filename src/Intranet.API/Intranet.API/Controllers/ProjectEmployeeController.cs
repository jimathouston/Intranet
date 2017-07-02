using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Intranet.API.Domain.Data;
using Intranet.API.Domain.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Intranet.API.ViewModels;

namespace Intranet.API.Controllers
{
    /// <summary>
    /// Manage what projects an employee is assigned to. Referred to below as assignment.
    /// </summary>
    [Produces("application/json")]
    [Route("/api/v1/profile/{profileId:int}/[controller]")]
    public class ProjectEmployeeController : Controller, IEditProfileController<ProjectEmployee>
    {
        private readonly IntranetApiContext _intranetApiContext;

        public ProjectEmployeeController(IntranetApiContext intranetApiContext)
        {
            _intranetApiContext = intranetApiContext;
        }

        /// <summary>
        /// Retrieve all assignments for a specific employee.
        /// </summary>
        /// <param name="profileId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get(int profileId)
        {
            try
            {
                var assignments = _intranetApiContext.ProjectEmployees.Where(e => e.EmployeeId == profileId).ToList();

                if (assignments.Count == 0)
                {
                    var error = Json("Could not find any assignment for user with id: " + profileId);
                    return NotFound(error);
                }

                var client = _intranetApiContext.Clients.ToList();
                var project = _intranetApiContext.Projects.ToList();
                var role = _intranetApiContext.Roles.ToList();

                IList<ProjectEmployeeViewModel> assignmentList = new List<ProjectEmployeeViewModel>();

                foreach (var assignment in assignments)
                {
                    var assignmentWithDescription = new ProjectEmployeeViewModel();

                    assignmentWithDescription.EmployeeId = profileId;
                    assignmentWithDescription.ProjectId = assignment.ProjectId;
                    assignmentWithDescription.RoleId = assignment.RoleId;
                    assignmentWithDescription.InformalProjectDescription = assignment.InformalDescription;
                    assignmentWithDescription.StartDate = assignment.StartDate;
                    assignmentWithDescription.EndDate = assignment.EndDate;
                    assignmentWithDescription.Active = assignment.Active;

                    assignmentWithDescription.ProjectName = project.Find(p => p.Id == assignment.ProjectId).Name;
                    assignmentWithDescription.ClientName = client.Find(c => c.Id == project.Find(p => p.Id == assignment.ProjectId).ClientId).Name;
                    assignmentWithDescription.RoleDescription = role.Find(r => r.Id == assignment.RoleId).Description;

                    assignmentList.Add(assignmentWithDescription);
                }

                var orderedAssignmentList = assignmentList.OrderBy(s => s.StartDate);

                return Ok(orderedAssignmentList);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Retrieve a specific assignment for an employee.
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [Route("{projectId:int}")]
        [HttpGet]
        public IActionResult Get(int profileId, int projectId)
        {
            try
            {
                var assignment = _intranetApiContext.ProjectEmployees.SingleOrDefault(p => p.EmployeeId == profileId && p.ProjectId == projectId);

                if (assignment == null)
                {
                    var error = Json("Could not find any assignment for user with id: " + profileId);
                    return NotFound(error);
                }

                var client = _intranetApiContext.Clients.ToList();
                var project = _intranetApiContext.Projects.ToList();
                var role = _intranetApiContext.Roles.ToList();

                var assignmentWithDescription = new ProjectEmployeeViewModel()
                {
                    EmployeeId = profileId,
                    ProjectId = assignment.ProjectId,
                    RoleId = assignment.RoleId,
                    InformalProjectDescription = assignment.InformalDescription,
                    StartDate = assignment.StartDate,
                    EndDate = assignment.EndDate,
                    Active = assignment.Active,

                    ProjectName = project.Find(p => p.Id == assignment.ProjectId).Name,
                    ClientName = client.Find(c => c.Id == project.Find(p => p.Id == assignment.ProjectId).ClientId).Name,
                    RoleDescription = role.Find(r => r.Id == assignment.RoleId).Description
                };

                return Ok(assignmentWithDescription);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Add a new assignment for an employee.
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post(int profileId, [FromBody] ProjectEmployee body)
        {
            try
            {
                var dupeCheck = _intranetApiContext.ProjectEmployees.SingleOrDefault(e => e.EmployeeId == profileId && e.ProjectId == body.ProjectId);
                if (dupeCheck != null)
                {
                    ModelState.AddModelError("SkillId", "The assignment already exists for this employee.");
                }

                var projectClientId = _intranetApiContext.Projects.SingleOrDefault(p => p.Id == body.ProjectId).ClientId;

                var clientExist = _intranetApiContext.Clients.SingleOrDefault(c => c.Id == projectClientId) != null;
                var projectExist = _intranetApiContext.Projects.SingleOrDefault(p => p.Id == body.ProjectId) != null;
                var roleExist = _intranetApiContext.Roles.SingleOrDefault(r => r.Id == body.RoleId) != null;

                if (!clientExist || !projectExist || !roleExist)
                {
                    ModelState.AddModelError("", "Invalid input");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                body.EmployeeId = profileId;

                _intranetApiContext.Add(body);
                _intranetApiContext.SaveChanges();

                return Ok(ModelState);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Change contents of an assignment.
        /// Can be changed: Project, role, informal assignment description,
        /// assignment status active or not and start/end date.
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="projectId"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("{projectId:int}")]
        [HttpPut]
        public IActionResult Put(int profileId, int projectId, [FromBody] ProjectEmployee body)
        {
            try
            {
                var projectClientId = _intranetApiContext.Projects.SingleOrDefault(p => p.Id == projectId).ClientId;

                var clientExist = _intranetApiContext.Clients.SingleOrDefault(c => c.Id == projectClientId) != null;
                var projectExist = _intranetApiContext.Projects.SingleOrDefault(p => p.Id == projectId) != null;
                var roleExist = _intranetApiContext.Roles.SingleOrDefault(r => r.Id == body.RoleId) != null;

                if (!clientExist || !projectExist || !roleExist)
                {
                    ModelState.AddModelError("", "Invalid input");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var projectToUpdate = _intranetApiContext.ProjectEmployees.SingleOrDefault(e => e.EmployeeId == profileId && e.ProjectId == projectId);
                if (projectToUpdate == null)
                {
                    var error = Json("Could not find any assignment to update.");
                    return NotFound(error);
                }

                projectToUpdate.RoleId = body.RoleId;
                projectToUpdate.InformalDescription = body.InformalDescription;
                projectToUpdate.Active = body.Active;
                projectToUpdate.StartDate = body.StartDate;
                projectToUpdate.EndDate = body.EndDate;
                _intranetApiContext.SaveChanges();

                return Ok(ModelState);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Remove an assignment from an employee.
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [Route("{projectId:int}")]
        [HttpDelete]
        public IActionResult Delete(int profileId, int projectId)
        {
            try
            {
                var assignmentToDelete = _intranetApiContext.ProjectEmployees.SingleOrDefault(e => e.EmployeeId == profileId && e.ProjectId == projectId);

                if (assignmentToDelete == null)
                {
                    var error = Json("Could not find employee skill to delete.");
                    return NotFound(error);
                }

                _intranetApiContext.ProjectEmployees.Remove(assignmentToDelete);
                _intranetApiContext.SaveChanges();

                return Ok(assignmentToDelete);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
