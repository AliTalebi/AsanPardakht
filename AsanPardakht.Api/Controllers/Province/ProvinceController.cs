using Microsoft.AspNetCore.Mvc;
using AsanPardakht.Core.Dipatcher;
using AsanPardakht.Queries.Queries.Provinces;
using AsanPardakht.Application.Commands.Provinces;
using AsanPardakht.Api.Controllers.Province.ViewModels;

namespace AsanPardakht.Api.Controllers.Province
{
    [Route("province")]
    public class ProvinceController : Controller
    {
        private readonly IDispatcher _dispatcher;
        public ProvinceController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetProvinceListQuery query)
        {
            var queryResult = await _dispatcher.ExecuteQueryAsync(query);

            return queryResult.Match<IActionResult>(Ok, error => BadRequest(error));
        }

        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetById([FromRoute] GetProvinceByIdQuery query)
        {
            var queryResult = await _dispatcher.ExecuteQueryAsync(query);

            return queryResult.Match<IActionResult>(response => response.Data == null ? NotFound() : Ok(response), error => BadRequest(error));
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateProvinceViewModel viewModel)
        {
            CreateProvinceCommand command = new(viewModel.Name);

            var createProvinceResult = await _dispatcher.DispachAsync(command);

            return createProvinceResult.Match<IActionResult>(success => Ok(), error => BadRequest(error));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put([FromRoute] int id, UpdateProvinceNameViewModel viewModel)
        {
            UpdateProvinceNameCommand command = new(id, viewModel.Name);

            var createProvinceResult = await _dispatcher.DispachAsync(command);

            return createProvinceResult.Match<IActionResult>(success => Ok(), error => BadRequest(error));
        }


        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Patch([FromRoute] int id, UpdateProvinceNameViewModel viewModel)
        {
            UpdateProvinceNameCommand command = new(id, viewModel.Name);

            var updateProvinceNameResult = await _dispatcher.DispachAsync(command);

            return updateProvinceNameResult.Match<IActionResult>(success => Ok(), error => BadRequest(error));
        }
    }
}
