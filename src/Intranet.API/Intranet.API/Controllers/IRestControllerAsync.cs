using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Intranet.API.Controllers
{
    /// <summary>
    /// Interface for implementing a RESTful controller
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRestControllerAsync<T>
    {
        /// <summary>
        /// Get all resources
        /// </summary>
        /// <returns></returns>
        Task<IActionResult> GetAsync();

        /// <summary>
        /// Get resource by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IActionResult> GetAsync(int id);

        /// <summary>
        /// Create a new resource
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        Task<IActionResult> PostAsync([FromBody] T body);

        /// <summary>
        /// Create or update a resource
        /// </summary>
        /// <param name="id"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        Task<IActionResult> PutAsync(int id, [FromBody] T body);

        /// <summary>
        /// Delete a resource by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IActionResult> DeleteAsync(int id);
    }
}