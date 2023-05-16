using OneOf;
using AsanPardakht.Core.Data;
using AsanPardakht.Core.Errors;
using AsanPardakht.Core.Command;
using AsanPardakht.Core.Security;
using AsanPardakht.Domain.Cities;
using AsanPardakht.Domain.People;
using AsanPardakht.Domain.Provinces;
using AsanPardakht.Domain.People.Repository;

namespace AsanPardakht.Application.Commands.People
{
    public record AddAddressToPersonCommand(int ProvinceId, int CityId, string? Detail) : IBaseCommand;
    public sealed class AddAddressToPersonCommandHandler : ICommandHandler<AddAddressToPersonCommand>
    {
        private readonly IRepository<City, CityId> _cityRepository;
        private readonly IUserIdentityAccessor _userIdentityAccessor;
        private readonly IPeopleRepository _personRepository;
        private readonly IRepository<Province, ProvinceId> _provinceRepository;

        public AddAddressToPersonCommandHandler(IPeopleRepository PersonRepository
            , IRepository<City, CityId> cityRepository, IRepository<Province, ProvinceId> provinceRepository
            , IUserIdentityAccessor userIdentityAccessor)
        {
            _cityRepository = cityRepository;
            _personRepository = PersonRepository;
            _provinceRepository = provinceRepository;
            _userIdentityAccessor = userIdentityAccessor;
        }

        public async Task<OneOf<bool, Error>> ExecuteAsync(AddAddressToPersonCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
            {
                return ApplicationErrors.DataIsInvalid;
            }

            Person? person = await _personRepository.GetByNationalCodeAsync(_userIdentityAccessor.GetNationalCode(), cancellationToken);

            if (person == null)
            {
                return ApplicationErrors.DataNotFound;
            }

            var city = await _cityRepository.GetByIdAsync(new CityId(command.CityId), cancellationToken);
            var province = await _provinceRepository.GetByIdAsync(new ProvinceId(command.ProvinceId), cancellationToken);

            return person.AddAddress(province, city, command.Detail);
        }
    }

}
