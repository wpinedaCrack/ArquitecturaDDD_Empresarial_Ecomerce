using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pacagroup.Ecommerce.Application.Interface;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.RateLimiting;

namespace Pacagroup.Ecommerce.Services.WebApi.Controllers.v2
{
    //[EnableRateLimiting("fixedWindow")]
    [Authorize]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2.0")]
    public class CategoriesController : Controller//ControllerBase
    {
        private readonly ICategoriesApplication _categoriesApplication;
        public CategoriesController(ICategoriesApplication categoriesApplication)
        {
            _categoriesApplication = categoriesApplication;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _categoriesApplication.GetAll();
            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response.Message);
        }
    }
}
