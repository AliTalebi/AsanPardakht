using AsanPardakht.Core.Query;
using AsanPardakht.Core.Errors;
using Microsoft.AspNetCore.Mvc;
using AsanPardakht.Core.Dipatcher;
using AsanPardakht.Queries.Queries.People;
using AsanPardakht.Application.Commands.People;
using AsanPardakht.Api.Controllers.People.ViewModels;

namespace AsanPardakht.Api.Controllers.Person
{
    /// <summary>
    /// اطلاعات اشخاص
    /// </summary>
    [Route("people")]
    public class PeopleController : Controller
    {
        private readonly IDispatcher _dispatcher;
        public PeopleController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        /// <summary>
        /// بازیابی لیست اشخاص
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(PagingQueryResponse<GetPeopleListQueryResult>), 200)]
        [ProducesErrorResponseType(typeof(Error))]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetPeopleListQuery query)
        {
            var queryResult = await _dispatcher.ExecuteQueryAsync(query);

            return queryResult.Match<IActionResult>(Ok, error => BadRequest(error));
        }

        /// <summary>
        /// بازیابی اطلاعات یک شخص خاص با شناسه
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(QueryResponse<GetPersonByIdQueryResult>), 200)]
        [ProducesErrorResponseType(typeof(Error))]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetById([FromRoute] GetPersonByIdQuery query)
        {
            var queryResult = await _dispatcher.ExecuteQueryAsync(query);

            return queryResult.Match<IActionResult>(response => response.Data == null ? NotFound() : Ok(response), error => BadRequest(error));
        }


        /// <summary>
        /// ثبت اطلاعات یک شخص
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [ProducesErrorResponseType(typeof(Error))]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [HttpPost]
        public async Task<IActionResult> Post(CreatePersonViewModel viewModel)
        {
            CreatePersonCommand command = new(viewModel.Name, viewModel.NationalCode);

            var createPersonResult = await _dispatcher.DispachAsync(command);

            return createPersonResult.Match<IActionResult>(success => Ok(), error => BadRequest(error));
        }

        /// <summary>
        /// ویرایش اطلاعات یک شخص
        /// </summary>
        /// <param name="id"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [ProducesErrorResponseType(typeof(Error))]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put([FromRoute] int id, UpdatePersonViewModel viewModel)
        {
            UpdatePersonCommand command = new(id, viewModel.Name, viewModel.NationalCode);

            var createPersonResult = await _dispatcher.DispachAsync(command);

            return createPersonResult.Match<IActionResult>(success => Ok(), error => BadRequest(error));
        }
    }
}
