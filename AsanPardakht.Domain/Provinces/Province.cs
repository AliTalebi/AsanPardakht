using OneOf;
using AsanPardakht.Core.Domain;
using AsanPardakht.Core.Errors;
using System.Diagnostics.CodeAnalysis;
using AsanPardakht.Domain.Provinces.Events;
using AsanPardakht.Domain.Provinces.DomainServices;

namespace AsanPardakht.Domain.Provinces
{
    public sealed class Province : AggregateRoot<ProvinceId>
    {
        private Province(ProvinceId id, string? name)
        {
            Id = id;
            Name = name;

            AddEvent(new ProvinceCreated(Id, Name));
        }

        private Province() { }

        [NotNull]
        public string? Name { get; private set; }
        public override ProvinceId Id { get; protected set; }

        public static async Task<OneOf<Province, Error>> CreateAsync(ProvinceId id, string? name, IProvinceExistByNameDomainService provinceExistByNameDomainService, CancellationToken cancellationToken = default!)
        {
            if (NameIsNullOrWhiteSpace(name))
            {
                return ApplicationErrors.Province.NameIsRequired;
            }

            if (!EnsureNameIsAtMost100Characters(name))
            {
                return ApplicationErrors.Province.NameMustBeAtMost100Characters;
            }

            if (await NameExistAsync(name, provinceExistByNameDomainService, cancellationToken))
            {
                return ApplicationErrors.Province.NameAlreadyTaken;
            }

            return new Province(id, name);
        }

        public async Task<OneOf<bool, Error>> UpdateNameAsync(string? name, IProvinceExistByNameDomainService provinceExistByNameDomainService, CancellationToken cancellationToken = default!)
        {
            if (NameIsNullOrWhiteSpace(name))
            {
                return ApplicationErrors.Province.NameIsRequired;
            }

            if (!EnsureNameIsAtMost100Characters(name))
            {
                return ApplicationErrors.Province.NameMustBeAtMost100Characters;
            }

            if (Name.ToLower() != name!.ToLower() && await NameExistAsync(name, provinceExistByNameDomainService, cancellationToken))
            {
                return ApplicationErrors.Province.NameAlreadyTaken;
            }

            Name = name;

            AddEvent(new ProvinceNameUpdated(Id, name));

            return true;
        }

        #region  validations

        private static bool NameIsNullOrWhiteSpace(string? name)
        {
            return string.IsNullOrWhiteSpace(name);
        }
        private static bool EnsureNameIsAtMost100Characters(string? name)
        {
            return !string.IsNullOrWhiteSpace(name) && name.Length <= 100;
        }
        private static async Task<bool> NameExistAsync(string? name, IProvinceExistByNameDomainService provinceExistByNameDomainService, CancellationToken cancellationToken = default!)
        {
            return await provinceExistByNameDomainService.IsNameExistAsync(name, cancellationToken);
        }

        #endregion
    }
}