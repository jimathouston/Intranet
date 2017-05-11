using Microsoft.AspNetCore.Mvc;

namespace Intranet.API.Controllers
{
  /// <summary>
  /// Interface for implementing a RESTful controller
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public interface IRestController<T>
  {
    /// <summary>
    /// Get all resources
    /// </summary>
    /// <returns></returns>
    IActionResult Get();

    /// <summary>
    /// Get resource by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    IActionResult Get(int id);

    /// <summary>
    /// Create a new resource
    /// </summary>
    /// <param name="body"></param>
    /// <returns></returns>
    IActionResult Post([FromBody] T body);

    /// <summary>
    /// Create or update a resource
    /// </summary>
    /// <param name="id"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    IActionResult Put(int id, [FromBody] T body);

    /// <summary>
    /// Delete a resource by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    IActionResult Delete(int id);
  }
}