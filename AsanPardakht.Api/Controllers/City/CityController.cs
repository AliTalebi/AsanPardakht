using Microsoft.AspNetCore.Mvc;
using AsanPardakht.Core.Dipatcher;
using AsanPardakht.Application.Commands.Cities;
using AsanPardakht.Api.Controllers.City.ViewModels;
using AsanPardakht.Queries.Queries.Cities;

namespace AsanPardakht.Api.Controllers.City
{
    [Route("city")]
    public class CityController : Controller
    {
        private readonly IDispatcher _dispatcher;
        public CityController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetCityListQuery query)
        {
            var queryResult = await _dispatcher.ExecuteQueryAsync(query);

            return queryResult.Match<IActionResult>(Ok, error => BadRequest(error));
        }

        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetById([FromRoute] GetCityByIdQuery query)
        {
            var queryResult = await _dispatcher.ExecuteQueryAsync(query);

            return queryResult.Match<IActionResult>(response => response.Data == null ? NotFound() : Ok(response), error => BadRequest(error));
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateCityViewModel viewModel)
        {
            CreateCityCommand command = new(viewModel.ProvinceId, viewModel.Name);

            var createCityResult = await _dispatcher.DispachAsync(command);

            return createCityResult.Match<IActionResult>(success => Ok(), error => BadRequest(error));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put([FromRoute] int id, UpdateCityNameViewModel viewModel)
        {
            UpdateCityNameCommand command = new(id, viewModel.Name);

            var createCityResult = await _dispatcher.DispachAsync(command);

            return createCityResult.Match<IActionResult>(success => Ok(), error => BadRequest(error));
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Patch([FromRoute] int id, UpdateCityNameViewModel viewModel)
        {
            UpdateCityNameCommand command = new(id, viewModel.Name);

            var updateCityNameResult = await _dispatcher.DispachAsync(command);

            return updateCityNameResult.Match<IActionResult>(success => Ok(), error => BadRequest(error));
        }
    }
}
