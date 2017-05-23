using Microsoft.AspNetCore.Mvc;

namespace Intranet.API.Controllers
{
    public interface IEditProfileController<T>
    {
    /// <summary>
    /// Get all resources by first id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    IActionResult Get(int id);

    /// <summary>
    /// Get resource by first and second id
    /// </summary>
    /// <param name="firstId"></param>
    /// <param name="secondId"></param>
    /// <returns></returns>
    IActionResult Get(int firstId, int secondId);

    /// <summary>
    /// Create a new resource
    /// </summary>
    /// <param name="id"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    IActionResult Post(int id, [FromBody] T body);

    /// <summary>
    /// Create or update a resource
    /// </summary>
    /// <param name="firstId"></param>
    /// <param name="secondId"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    IActionResult Put(int firstId, int secondId, [FromBody] T body);

    /// <summary>
    /// Delete a resource by first and second id
    /// </summary>
    /// <param name="firstId"></param>
    /// <param name="secondId"></param>
    /// <returns></returns>
    IActionResult Delete(int firstId, int secondId);
  }
}
