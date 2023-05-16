using OneOf;
using AsanPardakht.Core.Data;
using AsanPardakht.Core.Errors;
using AsanPardakht.Core.Command;
using AsanPardakht.Domain.Provinces;
using AsanPardakht.Domain.Provinces.DomainServices;

namespace AsanPardakht.Application.Commands.Provinces
{
    public record CreateProvinceCommand(string? Name) : IBaseCommand;
    public sealed class CreateProvinceCommandHandler : ICommandHandler<CreateProvinceCommand>
    {
        private readonly IRepository<Province, ProvinceId> _provinceRepository;
        private readonly IProvinceExistByNameDomainService _provinceExistByNameDomainService;

        public CreateProvinceCommandHandler(IRepository<Province, ProvinceId> provinceRepository, IProvinceExistByNameDomainService provinceExistByNameDomainService)
        {
            _provinceRepository = provinceRepository;
            _provinceExistByNameDomainService = provinceExistByNameDomainService;
        }

        public async Task<OneOf<bool, Error>> ExecuteAsync(CreateProvinceCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
            {
                return ApplicationErrors.DataIsInvalid;
            }

            var provinceId = await _provinceRepository.GetNewIdAsync();

            var provinceCreationResult = await Province.CreateAsync(provinceId, command.Name, _provinceExistByNameDomainService, cancellationToken);

            return provinceCreationResult.Match<OneOf<bool, Error>>(province =>
            {
                _provinceRepository.Insert(province);

                return true;
            }, error => error);
        }
    }

}
