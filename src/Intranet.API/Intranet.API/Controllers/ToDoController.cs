using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Intranet.API.Domain.Data;
using Intranet.API.Domain.Models.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Intranet.API.Controllers
{
  [Produces("application/json")]
  [Route("/api/v1/[controller]")]
  public class ToDoController : Controller, IRestController<ToDo>
  {
    private readonly IntranetApiContext _intranetApiContext;

    public ToDoController(IntranetApiContext intranetApiContext)
    {
      _intranetApiContext = intranetApiContext;
    }

    [AllowAnonymous]      // TODO this line is temporary for local testing without authentication, to be removed
    [Route("{id:int}")]
    [HttpDelete]
    public IActionResult Delete(int id)
    {
      try
      {
        if (id == 0)
        {
          return BadRequest(id);
        }

        var removeToDo = _intranetApiContext.ToDos.Find(id);

        if (removeToDo == null)
        {
          return NotFound(id);
        }

        _intranetApiContext.ToDos.Remove(removeToDo);
        _intranetApiContext.SaveChanges();

        return Ok(id);
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError);
      }
    }

    [AllowAnonymous]      // TODO this line is temporary for local testing without authentication, to be removed
    [Route("{id:int}")]
    [HttpGet]
    public IActionResult Get(int id)
    {
      try
      {
        var checklistTask = _intranetApiContext.ToDos.Find(id);

        if (checklistTask == null)
        {
          return NotFound(new ToDo());
        }

        return Ok(checklistTask);
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, new ToDo());
      }
    }

    [AllowAnonymous]      // TODO this line is temporary for local testing without authentication, to be removed
    [HttpGet]
    public IActionResult Get()
    {
      try
      {
        var toDoList = _intranetApiContext.ToDos.ToList();

        if (toDoList == null)
        {
          return NotFound(new ToDo());
        }

        return Ok(toDoList);
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, new ToDo());
      }
    }

    [AllowAnonymous]      // TODO this line is temporary for local testing without authentication, to be removed
    [HttpPost]
    public IActionResult Post([FromBody] ToDo newItem)
    {
      try
      {
        if (!ModelState.IsValid)
        {
          return BadRequest(ModelState);
        }

        var newToDo = new ToDo()
        {
          Description = newItem.Description
        };

        _intranetApiContext.ToDos.Add(newToDo);
        _intranetApiContext.SaveChanges();

        return Ok(ModelState);
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError);
      }
    }

    [AllowAnonymous]      // TODO this line is temporary for local testing without authentication, to be removed
    [Route("{id:int}")]
    [HttpPut]
    public IActionResult Put(int id, [FromBody] ToDo update)
    {
      try
      {
        if (id == 0)
        {
          ModelState.AddModelError(nameof(ToDo.Id), "");
        }

        if (!ModelState.IsValid)
        {
          return BadRequest(ModelState);
        }

        var updateToDo = _intranetApiContext.ToDos.Find(id);

        if (updateToDo == null)
        {
          return NotFound(ModelState);
        }

        updateToDo.Description = update.Description;

        _intranetApiContext.SaveChanges();

        return Ok(ModelState);
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError);
      }
    }
  }
}