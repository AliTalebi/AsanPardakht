using OneOf;
using AsanPardakht.Core.Data;
using AsanPardakht.Core.Errors;
using AsanPardakht.Core.Command;
using AsanPardakht.Domain.Provinces;
using AsanPardakht.Domain.Provinces.DomainServices;

namespace AsanPardakht.Application.Commands.Provinces
{
    public record UpdateProvinceNameCommand(int Id, string? Name) : IBaseCommand;
    public sealed class UpdateProvinceNameCommandHandler : ICommandHandler<UpdateProvinceNameCommand>
    {
        private readonly IRepository<Province, ProvinceId> _provinceRepository;
        private readonly IProvinceExistByNameDomainService _provinceExistByNameDomainService;

        public UpdateProvinceNameCommandHandler(IRepository<Province, ProvinceId> provinceRepository, IProvinceExistByNameDomainService provinceExistByNameDomainService)
        {
            _provinceRepository = provinceRepository;
            _provinceExistByNameDomainService = provinceExistByNameDomainService;
        }

        public async Task<OneOf<bool, Error>> ExecuteAsync(UpdateProvinceNameCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
            {
                return ApplicationErrors.DataIsInvalid;
            }

            Province? province = await _provinceRepository.GetByIdAsync(new ProvinceId(command.Id), cancellationToken);

            if (province == null)
            {
                return ApplicationErrors.DataNotFound;
            }

            return await province.UpdateNameAsync(command.Name, _provinceExistByNameDomainService, cancellationToken);
        }
    }

}
