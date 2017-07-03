using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Intranet.API.Domain.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Intranet.API.Domain.Data;
using Intranet.API.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Intranet.API.Controllers
{
    /// <summary>
    /// Manage the skills of all employees.
    /// </summary>
    [Produces("application/json")]
    [Route("/api/v1/profile/{profileId:int}/[controller]")]
    public class EmployeeSkillController : Controller, IEditProfileController<EmployeeSkill>
    {
        private readonly IntranetApiContext _intranetApiContext;

        public EmployeeSkillController(IntranetApiContext intranetApiContext)
        {
            _intranetApiContext = intranetApiContext;
        }

        /// <summary>
        /// Retrieve a list of all skills for a specific employee.
        /// </summary>
        /// <param name="profileId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get(int profileId)
        {
            try
            {
                var employeeSkills = _intranetApiContext.EmployeeSkills.Where(e => e.EmployeeId == profileId).ToList();

                if (employeeSkills.Count == 0)
                {
                    var error = Json("Could not find any skills for user with id: " + profileId);
                    return NotFound(error);
                }

                var skillType = _intranetApiContext.Skills.ToList();
                var skillLevel = _intranetApiContext.SkillLevels.ToList();

                IList<EmployeeSkillViewModel> skillList = new List<EmployeeSkillViewModel>();

                foreach (var skill in employeeSkills)
                {
                    var skillWithDescription = new EmployeeSkillViewModel();

                    skillWithDescription.EmployeeId = profileId;
                    skillWithDescription.SkillId = skill.SkillId;
                    skillWithDescription.CurrentId = skill.CurrentLevel;
                    //skillWithDescription.DesiredId = skill.DesiredLevel;

                    skillWithDescription.SkillDescription = skillType.Find(s => s.Id == skill.SkillId).Description;
                    skillWithDescription.CurrentDescription = skillLevel.Find(l => l.Id == skill.CurrentLevel).Description;
                    //skillWithDescription.DesiredDescription = skillLevel.Find(d => d.Id == skill.DesiredLevel).Description;

                    skillList.Add(skillWithDescription);
                }

                var orderedSkillList = skillList.OrderBy(s => s.SkillId);

                return Ok(orderedSkillList);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Retrieve a specific skill of an employee.
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="skillId"></param>
        /// <returns></returns>
        [Route("{skillId:int}")]
        [HttpGet]
        public IActionResult Get(int profileId, int skillId)
        {
            try
            {
                var employeeSkill = _intranetApiContext.EmployeeSkills.SingleOrDefault(e => e.EmployeeId == profileId && e.SkillId == skillId);

                if (employeeSkill == null)
                {
                    var error = Json("Could not find any skill by user id: " + profileId + " and skill id: " + skillId);
                    return NotFound(error);
                }

                var skillType = _intranetApiContext.Skills.ToList();
                var skillLevel = _intranetApiContext.SkillLevels.ToList();

                var skillWithDescription = new EmployeeSkillViewModel()
                {
                    EmployeeId = profileId,
                    SkillId = skillId,
                    CurrentId = employeeSkill.CurrentLevel,
                    //DesiredId = employeeSkill.DesiredLevel,

                    SkillDescription = skillType.Find(s => s.Id == skillId).Description,
                    CurrentDescription = skillLevel.Find(c => c.Id == employeeSkill.CurrentLevel).Description,
                    //DesiredDescription = skillLevel.Find(d => d.Id == employeeSkill.DesiredLevel).Description
                };

                return Ok(skillWithDescription);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Add new skill to an employee. Assign the current and desired level of skill.
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="skill"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post(int profileId, [FromBody] EmployeeSkill skill)
        {
            try
            {
                var dupeCheck = _intranetApiContext.EmployeeSkills.SingleOrDefault(e => e.EmployeeId == profileId && e.SkillId == skill.SkillId);
                if (dupeCheck != null)
                {
                    ModelState.AddModelError("SkillId", "The skill have already been added for this employee.");
                }

                var skillExist = _intranetApiContext.Skills.SingleOrDefault(s => s.Id == skill.SkillId) != null;
                var skillLevelCurrExist = _intranetApiContext.SkillLevels.SingleOrDefault(l => l.Id == skill.CurrentLevel) != null;
                //var skillLevelDesiExist = _intranetApiContext.SkillLevels.SingleOrDefault(l => l.Id == skill.DesiredLevel) != null;
                if (!skillExist || !skillLevelCurrExist) //|| !skillLevelDesiExist)
                {
                    ModelState.AddModelError("", "Invalid input");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                skill.EmployeeId = profileId;

                _intranetApiContext.Add(skill);
                _intranetApiContext.SaveChanges();

                return Ok(ModelState);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Change the current and desired level of skill for an employee.
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="skillId"></param>
        /// <param name="skill">Form data containing the changed skill level provided by client</param>
        /// <returns></returns>
        [Route("{skillId:int}")]
        [HttpPut]
        public IActionResult Put(int profileId, int skillId, [FromBody] EmployeeSkill skill)
        {
            try
            {
                var skillExist = _intranetApiContext.Skills.SingleOrDefault(s => s.Id == skillId) != null;
                var skillLevelCurrExist = _intranetApiContext.SkillLevels.SingleOrDefault(l => l.Id == skill.CurrentLevel) != null;
                //var skillLevelDesiExist = _intranetApiContext.SkillLevels.SingleOrDefault(l => l.Id == skill.DesiredLevel) != null;
                if (!skillExist || !skillLevelCurrExist)// || !skillLevelDesiExist)
                {
                    ModelState.AddModelError("", "Invalid input");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var skillToUpdate = _intranetApiContext.EmployeeSkills.SingleOrDefault(e => e.EmployeeId == profileId && e.SkillId == skillId);
                if (skillToUpdate == null)
                {
                    var error = Json("Could not find employee skill to update.");
                    return NotFound(error);
                }

                skillToUpdate.CurrentLevel = skill.CurrentLevel;
                //skillToUpdate.DesiredLevel = skill.DesiredLevel;
                _intranetApiContext.SaveChanges();

                return Ok(ModelState);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Remove a specific skill from an employee.
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="skillId"></param>
        /// <returns></returns>
        [Route("{skillId:int}")]
        [HttpDelete]
        public IActionResult Delete(int profileId, int skillId)
        {
            try
            {
                var employeeSkillToDelete = _intranetApiContext.EmployeeSkills.SingleOrDefault(e => e.EmployeeId == profileId && e.SkillId == skillId);
                if (employeeSkillToDelete == null)
                {
                    var error = Json("Could not find employee skill to delete.");
                    return NotFound(error);
                }

                _intranetApiContext.EmployeeSkills.Remove(employeeSkillToDelete);
                _intranetApiContext.SaveChanges();

                return Ok(employeeSkillToDelete);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}