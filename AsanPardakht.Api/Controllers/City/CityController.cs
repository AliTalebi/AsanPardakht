using Microsoft.AspNetCore.Mvc;
using AsanPardakht.Core.Dipatcher;
using AsanPardakht.Application.Commands.Cities;
using AsanPardakht.Api.Controllers.City.ViewModels;
using AsanPardakht.Queries.Queries.Cities;
using AsanPardakht.Core.Query;
using AsanPardakht.Core.Errors;

namespace AsanPardakht.Api.Controllers.City
{
    /// <summary>
    /// اطلاعات شهرها
    /// </summary>
    [Route("city")]
    public class CityController : Controller
    {
        private readonly IDispatcher _dispatcher;
        public CityController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        /// <summary>
        /// بازیابی لیست شهرها
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(PagingQueryResponse<GetCityListQueryResult>), 200)]
        [ProducesErrorResponseType(typeof(Error))]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetCityListQuery query)
        {
            var queryResult = await _dispatcher.ExecuteQueryAsync(query);

            return queryResult.Match<IActionResult>(Ok, error => BadRequest(error));
        }

        /// <summary>
        /// بازیابی اطلاعات یک شهر خاص با شناسه
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(QueryResponse<GetCityByIdQueryResult>), 200)]
        [ProducesErrorResponseType(typeof(Error))]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetById([FromRoute] GetCityByIdQuery query)
        {
            var queryResult = await _dispatcher.ExecuteQueryAsync(query);

            return queryResult.Match<IActionResult>(response => response.Data == null ? NotFound() : Ok(response), error => BadRequest(error));
        }

        /// <summary>
        /// ثبت اطلاعات شهر
        /// در صورت وجود نام تکراری اطلاعات ثبت نخواهد شد
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [ProducesErrorResponseType(typeof(Error))]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [HttpPost]
        public async Task<IActionResult> Post(CreateCityViewModel viewModel)
        {
            CreateCityCommand command = new(viewModel.ProvinceId, viewModel.Name);

            var createCityResult = await _dispatcher.DispachAsync(command);

            return createCityResult.Match<IActionResult>(success => Ok(), error => BadRequest(error));
        }

        /// <summary>
        /// ویرایش نام یک شهر خاص با شناسه
        /// در صورت وجود نام تکراری اطلاعات ثبت نخواهد شد
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesErrorResponseType(typeof(Error))]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Patch([FromRoute] int id, UpdateCityNameViewModel viewModel)
        {
            UpdateCityNameCommand command = new(id, viewModel.Name);

            var updateCityNameResult = await _dispatcher.DispachAsync(command);

            return updateCityNameResult.Match<IActionResult>(success => Ok(), error => BadRequest(error));
        }
    }
}
