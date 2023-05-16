using OneOf;
using AsanPardakht.Core.Data;
using AsanPardakht.Core.Errors;
using AsanPardakht.Core.Command;
using AsanPardakht.Domain.People;
using AsanPardakht.Domain.People.DomainServices;

namespace AsanPardakht.Application.Commands.People
{
    public record CreatePersonCommand(string? Name, string? NationalCode) : IBaseCommand;
    public sealed class CreatePersonCommandHandler : ICommandHandler<CreatePersonCommand>
    {
        private readonly IRepository<Person, PersonId> _personRepository;
        private readonly IPersonExistByNationalCodeDomainService _personExistByNationalCodeDomainService;

        public CreatePersonCommandHandler(IRepository<Person, PersonId> PersonRepository, IPersonExistByNationalCodeDomainService personExistByNationalCodeDomainService)
        {
            _personRepository = PersonRepository;
            _personExistByNationalCodeDomainService = personExistByNationalCodeDomainService;
        }

        public async Task<OneOf<bool, Error>> ExecuteAsync(CreatePersonCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
            {
                return ApplicationErrors.DataIsInvalid;
            }

            var personId = await _personRepository.GetNewIdAsync();

            var personCreationResult = await Person.CreateAsync(personId, command.Name, command.NationalCode, _personExistByNationalCodeDomainService, cancellationToken);

            return personCreationResult.Match<OneOf<bool, Error>>(Person =>
            {
                _personRepository.Insert(Person);

                return true;
            }, error => error);
        }
    }

}
