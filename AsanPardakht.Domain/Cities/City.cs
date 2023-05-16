using OneOf;
using AsanPardakht.Core.Domain;
using AsanPardakht.Core.Errors;
using AsanPardakht.Domain.Cities.Events;
using AsanPardakht.Domain.Cities.DomainServices;
using AsanPardakht.Domain.Provinces;
using AsanPardakht.Domain.Provinces.DomainServices;
using System.Diagnostics.CodeAnalysis;

namespace AsanPardakht.Domain.Cities
{
    public sealed class City : AggregateRoot<CityId>
    {
        private City(CityId id, string? name, ProvinceId provinceId)
        {
            Id = id;
            Name = name;
            ProvinceId = provinceId;

            AddEvent(new CityCreated(Id, Name,ProvinceId));
        }

        private City() { }

        [NotNull]
        public string? Name { get; private set; }
        public override CityId Id { get; protected set; }
        public ProvinceId ProvinceId { get; private set; }

        public static async Task<OneOf<City, Error>> CreateAsync(CityId id, ProvinceId provinceId, string? name
            , ICityExistByNameDomainService cityExistNameDomainService, IProvinceExistByIdDomainService provinceExistByIdDomainService, CancellationToken cancellationToken = default!)
        {
            if (NameIsNullOrWhiteSpace(name))
            {
                return ApplicationErrors.City.NameIsRequired;
            }

            if (!EnsureNameIsAtMost100Characters(name))
            {
                return ApplicationErrors.City.NameMustBeAtMost100Characters;
            }

            if (await NameExistAsync(name, cityExistNameDomainService, cancellationToken))
            {
                return ApplicationErrors.City.NameAlreadyTaken;
            }

            if (!await provinceExistByIdDomainService.IsProvinceExistAsync(provinceId, cancellationToken))
            {
                return ApplicationErrors.City.ProvinceNotFound;
            }

            return new City(id, name, provinceId);
        }

        public async Task<OneOf<bool, Error>> UpdateNameAsync(string? name, ICityExistByNameDomainService cityExistNameDomainService, CancellationToken cancellationToken = default!)
        {
            if (NameIsNullOrWhiteSpace(name))
            {
                return ApplicationErrors.City.NameIsRequired;
            }

            if (!EnsureNameIsAtMost100Characters(name))
            {
                return ApplicationErrors.City.NameMustBeAtMost100Characters;
            }

            if (Name!.ToLower() != name!.ToLower() && await NameExistAsync(name, cityExistNameDomainService, cancellationToken))
            {
                return ApplicationErrors.City.NameAlreadyTaken;
            }

            Name = name;

            AddEvent(new CityNameUpdated(Id, name));

            return true;
        }

        private static bool EnsureNameIsAtMost100Characters(string? name)
        {
            return !string.IsNullOrWhiteSpace(name) && name.Length <= 100;
        }
        private static bool NameIsNullOrWhiteSpace(string? name)
        {
            return string.IsNullOrWhiteSpace(name);
        }
        private static async Task<bool> NameExistAsync(string? name, ICityExistByNameDomainService cityExistNameDomainService, CancellationToken cancellationToken = default!)
        {
            return await cityExistNameDomainService.IsNameExistAsync(name, cancellationToken);
        }
    }
}