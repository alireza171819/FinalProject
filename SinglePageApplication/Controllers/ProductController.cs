using ApplicationService.Dtos.ProductDtos;
using ApplicationService.ProductServices.Contracts;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace SinglePageApplication.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApplicationService _productApplicationService;

        public ProductController(IProductApplicationService productApplicationService)
        {
            _productApplicationService = productApplicationService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAll()
        {
            var response = await _productApplicationService.GetAll();
            if (!response.IsSuccessful)
            {
                return View(new GetAllProductDto { GetByIdProductDtos = new List<GetByIdProductDto>() });
            }
            return Json(response.Result);
        }

        #region [- Create() -]
        /// <summary>
        /// Creates a new product in the system (form submission).
        /// </summary>
        /// <param name="productCreate">DTO containing product details.</param>
        /// <returns>
        /// Redirects to <see cref="Index"/> on success, or redisplays the form with validation errors.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]PostProductDto productCreate)
        {
            if (!ModelState.IsValid)
                return BadRequest(productCreate);

            var response = await _productApplicationService.Post(productCreate);
            if (!response.IsSuccessful)
            {
                return BadRequest(response.ErrorMessage);
            }
            return Ok(response.Result);
        }


        #endregion

    }
}
