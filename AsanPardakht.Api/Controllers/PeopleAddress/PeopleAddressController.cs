using AsanPardakht.Core.Query;
using AsanPardakht.Core.Errors;
using Microsoft.AspNetCore.Mvc;
using AsanPardakht.Core.Dipatcher;
using AsanPardakht.Application.Commands.People;
using AsanPardakht.Queries.Queries.PeopleAddress;

namespace AsanPardakht.Api.Controllers.PersonAddress
{
    /// <summary>
    /// آدرس های کاربر جاری
    /// </summary>
    [Route("people-address")]
    public class PersonAddressController : Controller
    {
        private readonly IDispatcher _dispatcher;
        public PersonAddressController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        /// <summary>
        /// ثبت آدرس
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesErrorResponseType(typeof(Error))]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [HttpPost]
        public async Task<IActionResult> Post(AddAddressToPersonCommand command)
        {
            var updatePersonAddressNameResult = await _dispatcher.DispachAsync(command);

            return updatePersonAddressNameResult.Match<IActionResult>(success => Ok(), error => BadRequest(error));
        }

        /// <summary>
        /// بازیابی لیست آدرس ها
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(PagingQueryResponse<GetAddressListQueryResult>), 200)]
        [ProducesErrorResponseType(typeof(Error))]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAddressListQuery query)
        {
            var updatePersonAddressNameResult = await _dispatcher.ExecuteQueryAsync(query);

            return updatePersonAddressNameResult.Match<IActionResult>(Ok, error => BadRequest(error));
        }
    }
}