using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Intranet.API.Domain.Data;
using Intranet.API.Domain.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Intranet.API.Controllers
{
  [Produces("application/json")]
  [Route("/api/v1/[controller]")]
  public class ChecklistController : Controller
  {
    private readonly IntranetApiContext _intranetApiContext;

    public ChecklistController(IntranetApiContext intranetApiContext)
    {
      _intranetApiContext = intranetApiContext;
    }

    [AllowAnonymous]      // TODO this line is temporary for local testing without authentication, to be removed
    [Route("employee/{id:int}")]
    [HttpGet]
    // GET api/v1/checklist/employee/5 return an employee by id 5 and all related checklist ids 
    public IActionResult GetEmployeeById(int id)
   {
      try
      {
        var employee = _intranetApiContext.Employees.Find(id);

        if (employee == null)
        {
          return NotFound(new Employee());
        }

        _intranetApiContext.Entry(employee).Collection(e => e.ToDos).Load();

        return Ok(employee);
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, new Employee());
      }
    }

    [AllowAnonymous]      // TODO this line is temporary for local testing without authentication, to be removed
    [Route("{id:int}")]
    [HttpGet]
    // GET api/v1/checklist/1 return a checklist task by id and description 
    public IActionResult GetChecklistTaskById(int id)
    {
      try
      {
        var checklistTask = _intranetApiContext.Checklist.Find(id);

        if (checklistTask == null)
        {
          return NotFound(new Checklist());
        }

        return Ok(checklistTask);
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, new Checklist());
      }
    }
  }
}