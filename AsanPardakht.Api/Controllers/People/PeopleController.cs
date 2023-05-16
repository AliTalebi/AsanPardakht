using Microsoft.AspNetCore.Mvc;
using AsanPardakht.Core.Dipatcher;
using AsanPardakht.Application.Commands.People;
using AsanPardakht.Api.Controllers.People.ViewModels;
using AsanPardakht.Api.Controllers.Province.ViewModels;
using AsanPardakht.Queries.Queries.People;
using AsanPardakht.Queries.Queries.PeopleAddress;

namespace AsanPardakht.Api.Controllers.Person
{
    [Route("people")]
    public class PersonController : Controller
    {
        private readonly IDispatcher _dispatcher;
        public PersonController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetPeopleListQuery query)
        {
            var queryResult = await _dispatcher.ExecuteQueryAsync(query);

            return queryResult.Match<IActionResult>(Ok, error => BadRequest(error));
        }

        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetById([FromRoute] GetPersonByIdQuery query)
        {
            var queryResult = await _dispatcher.ExecuteQueryAsync(query);

            return queryResult.Match<IActionResult>(response => response.Data == null ? NotFound() : Ok(response), error => BadRequest(error));
        }


        [HttpPost]
        public async Task<IActionResult> Post(CreatePersonViewModel viewModel)
        {
            CreatePersonCommand command = new(viewModel.Name, viewModel.NationalCode);

            var createPersonResult = await _dispatcher.DispachAsync(command);

            return createPersonResult.Match<IActionResult>(success => Ok(), error => BadRequest(error));
        }
    }
}
