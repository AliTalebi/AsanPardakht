using OneOf;
using AsanPardakht.Core.Data;
using AsanPardakht.Core.Errors;
using AsanPardakht.Core.Command;
using AsanPardakht.Domain.Cities;
using AsanPardakht.Domain.Provinces;
using AsanPardakht.Domain.Cities.DomainServices;
using AsanPardakht.Domain.Provinces.DomainServices;

namespace AsanPardakht.Application.Commands.Cities
{
    public record CreateCityCommand(int ProvinceId, string? Name) : IBaseCommand;
    public sealed class CreateCityCommandHandler : ICommandHandler<CreateCityCommand>
    {
        private readonly IRepository<City, CityId> _cityRepository;
        private readonly ICityExistByNameDomainService _cityExistByNameDomainService;
        private readonly IProvinceExistByIdDomainService _provinceExistByIdDomainService;

        public CreateCityCommandHandler(IRepository<City, CityId> cityRepository, ICityExistByNameDomainService cityExistByNameDomainService, IProvinceExistByIdDomainService provinceExistByIdDomainService)
        {
            _cityRepository = cityRepository;
            _cityExistByNameDomainService = cityExistByNameDomainService;
            _provinceExistByIdDomainService = provinceExistByIdDomainService;
        }

        public async Task<OneOf<bool, Error>> ExecuteAsync(CreateCityCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
            {
                return ApplicationErrors.DataIsInvalid;
            }

            var cityId = await _cityRepository.GetNewIdAsync();

            var cityCreationResult = await City.CreateAsync(cityId, new ProvinceId(command.ProvinceId), command.Name, _cityExistByNameDomainService, _provinceExistByIdDomainService, cancellationToken);

            return cityCreationResult.Match<OneOf<bool, Error>>(city =>
            {
                _cityRepository.Insert(city);

                return true;
            }, error => error);
        }
    }

}
