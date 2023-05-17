using AsanPardakht.Api.Controllers.Province.ViewModels;
using AsanPardakht.Application.Commands.Provinces;
using AsanPardakht.Core.Dipatcher;
using AsanPardakht.Core.Errors;
using AsanPardakht.Core.Query;
using AsanPardakht.Queries.Queries.Provinces;
using Microsoft.AspNetCore.Mvc;

namespace AsanPardakht.Api.Controllers.Province
{
    /// <summary>
    /// اطلاعات استانها
    /// </summary>
    [Route("province")]
    public class ProvinceController : Controller
    {
        private readonly IDispatcher _dispatcher;
        public ProvinceController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        /// <summary>
        /// بازیابی لیست استانها
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(PagingQueryResponse<GetProvinceListQueryResult>), 200)]
        [ProducesErrorResponseType(typeof(Error))]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetProvinceListQuery query)
        {
            var queryResult = await _dispatcher.ExecuteQueryAsync(query);

            return queryResult.Match<IActionResult>(Ok, error => BadRequest(error));
        }

        /// <summary>
        /// بازیابی اطلاعات یک استان خاص با شناسه
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(QueryResponse<GetProvinceByIdQueryResult>), 200)]
        [ProducesErrorResponseType(typeof(Error))]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetById([FromRoute] GetProvinceByIdQuery query)
        {
            var queryResult = await _dispatcher.ExecuteQueryAsync(query);

            return queryResult.Match<IActionResult>(response => response.Data == null ? NotFound() : Ok(response), error => BadRequest(error));
        }

        /// <summary>
        /// ثبت اطلاعات استان
        /// در صورت وجود نام تکراری اطلاعات ثبت نخواهد شد
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [ProducesErrorResponseType(typeof(Error))]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [HttpPost]
        public async Task<IActionResult> Post(CreateProvinceViewModel viewModel)
        {
            CreateProvinceCommand command = new(viewModel.Name);

            var createProvinceResult = await _dispatcher.DispachAsync(command);

            return createProvinceResult.Match<IActionResult>(success => Ok(), error => BadRequest(error));
        }

        /// <summary>
        /// ویرایش نام یک استان خاص با شناسه
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
        public async Task<IActionResult> Patch([FromRoute] int id, UpdateProvinceNameViewModel viewModel)
        {
            UpdateProvinceNameCommand command = new(id, viewModel.Name);

            var updateProvinceNameResult = await _dispatcher.DispachAsync(command);

            return updateProvinceNameResult.Match<IActionResult>(success => Ok(), error => BadRequest(error));
        }
    }
}
