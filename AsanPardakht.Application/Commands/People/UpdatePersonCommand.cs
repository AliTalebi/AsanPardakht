using OneOf;
using AsanPardakht.Core.Data;
using AsanPardakht.Core.Errors;
using AsanPardakht.Core.Command;
using AsanPardakht.Domain.People;
using AsanPardakht.Domain.People.DomainServices;

namespace AsanPardakht.Application.Commands.People
{
    public record UpdatePersonCommand(int Id, string? Name, string? NationalCode) : IBaseCommand;
    public sealed class UpdatePersonCommandHandler : ICommandHandler<UpdatePersonCommand>
    {
        private readonly IRepository<Person, PersonId> _personRepository;
        private readonly IPersonExistByNationalCodeDomainService _personExistByNationalCodeDomainService;

        public UpdatePersonCommandHandler(IRepository<Person, PersonId> PersonRepository, IPersonExistByNationalCodeDomainService personExistByNationalCodeDomainService)
        {
            _personRepository = PersonRepository;
            _personExistByNationalCodeDomainService = personExistByNationalCodeDomainService;
        }

        public async Task<OneOf<bool, Error>> ExecuteAsync(UpdatePersonCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
            {
                return ApplicationErrors.DataIsInvalid;
            }

            Person? person = await _personRepository.GetByIdAsync(new PersonId(command.Id), cancellationToken);

            if(person == null)
            {
                return ApplicationErrors.DataNotFound;
            }

            return await person.UpdateAsync(command.Name, command.NationalCode, _personExistByNationalCodeDomainService, cancellationToken);
        }
    }

}
