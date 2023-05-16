using OneOf;
using AsanPardakht.Core.Data;
using AsanPardakht.Core.Errors;
using AsanPardakht.Core.Command;
using AsanPardakht.Domain.Cities;
using AsanPardakht.Domain.Cities.DomainServices;

namespace AsanPardakht.Application.Commands.Cities
{
    public record UpdateCityNameCommand(int Id, string? Name) : IBaseCommand;
    public sealed class UpdateCityNameCommandHandler : ICommandHandler<UpdateCityNameCommand>
    {
        private readonly IRepository<City, CityId> _cityRepository;
        private readonly ICityExistByNameDomainService _cityExistByNameDomainService;

        public UpdateCityNameCommandHandler(IRepository<City, CityId> cityRepository, ICityExistByNameDomainService cityExistByNameDomainService)
        {
            _cityRepository = cityRepository;
            _cityExistByNameDomainService = cityExistByNameDomainService;
        }

        public async Task<OneOf<bool, Error>> ExecuteAsync(UpdateCityNameCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
            {
                return ApplicationErrors.DataIsInvalid;
            }

            City? city = await _cityRepository.GetByIdAsync(new CityId(command.Id), cancellationToken);

            if (city == null)
            {
                return ApplicationErrors.DataNotFound;
            }

            return await city.UpdateNameAsync(command.Name, _cityExistByNameDomainService, cancellationToken);
        }
    }
}
