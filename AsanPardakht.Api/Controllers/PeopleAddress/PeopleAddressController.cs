using Microsoft.AspNetCore.Mvc;
using AsanPardakht.Core.Dipatcher;
using AsanPardakht.Application.Commands.People;
using AsanPardakht.Queries.Queries.PeopleAddress;

namespace AsanPardakht.Api.Controllers.PersonAddress
{
    [Route("people-address")]
    public class PersonAddressController : Controller
    {
        private readonly IDispatcher _dispatcher;
        public PersonAddressController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpPost]
        public async Task<IActionResult> Post(AddAddressToPersonCommand command)
        {
            var updatePersonAddressNameResult = await _dispatcher.DispachAsync(command);

            return updatePersonAddressNameResult.Match<IActionResult>(success => Ok(), error => BadRequest(error));
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAddressListQuery query)
        {
            var updatePersonAddressNameResult = await _dispatcher.ExecuteQueryAsync(query);

            return updatePersonAddressNameResult.Match<IActionResult>(Ok, error => BadRequest(error));
        }
    }
}