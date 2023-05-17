using OneOf;
using AsanPardakht.Core.Common;
using AsanPardakht.Core.Domain;
using AsanPardakht.Core.Errors;
using AsanPardakht.Domain.Cities;
using AsanPardakht.Domain.Provinces;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using AsanPardakht.Domain.People.Events;
using AsanPardakht.Domain.Cities.DomainServices;
using AsanPardakht.Domain.People.DomainServices;
using AsanPardakht.Domain.Provinces.DomainServices;

namespace AsanPardakht.Domain.People
{
    public sealed class Person : AggregateRoot<PersonId>
    {
        private Person(PersonId id, string? name, string? nationalCode)
        {
            Id = id;
            Name = name;
            NationalCode = nationalCode;

            AddEvent(new PersonCreated(Id, Name, NationalCode));
        }

        private Person() { }

        public HashSet<PersonAddress> _addresses = new();

        [NotNull]
        public string? Name { get; private set; }
        [NotNull]
        public string? NationalCode { get; private set; }
        public override PersonId Id { get; protected set; }

        public IReadOnlyCollection<PersonAddress> Addresses => new ReadOnlyCollection<PersonAddress>(_addresses.ToArray());
        public OneOf<bool, Error> AddAddress(Province? province, City? city, string? detail)
        {
            if (ValueIsNullOrWhiteSpace(detail))
            {
                return ApplicationErrors.People.AddressDetailIsRequired;
            }

            if (!EnsureAddressIsAtMost500Characters(detail))
            {
                return ApplicationErrors.People.AddressDetailMustBeAtMost500Characters;
            }

            if (province == null)
            {
                return ApplicationErrors.People.ProvinceNotFound;
            }

            if (city == null)
            {
                return ApplicationErrors.People.CityNotFound;
            }

            _addresses.Add(new(province.Id, city.Id, detail));

            AddEvent(new PersonAddressAdded(Id, province.Id, province.Name, city.Id, city.Name, detail));

            return true;
        }

        public static async Task<OneOf<Person, Error>> CreateAsync(PersonId id, string? name, string? nationalCode, IPersonExistByNationalCodeDomainService personDomainService, CancellationToken cancellationToken = default!)
        {
            if (ValueIsNullOrWhiteSpace(name))
            {
                return ApplicationErrors.People.NameIsRequired;
            }

            if (!EnsureNameIsAtMost100Characters(name))
            {
                return ApplicationErrors.People.NameMustBeAtMost100Characters;
            }

            if (!EnsureNationalCodeIsValid(nationalCode))
            {
                return ApplicationErrors.People.NationalCodeIsInvalid;
            }

            if (await IsNationalCodeExistAsync(nationalCode, personDomainService, cancellationToken))
            {
                return ApplicationErrors.People.NationalCodeAlreadyExist;
            }

            return new Person(id, name, nationalCode);
        }
        public async Task<OneOf<bool, Error>> UpdateAsync(string? name, string? nationalCode, IPersonExistByNationalCodeDomainService personDomainService, CancellationToken cancellationToken = default!)
        {
            if (ValueIsNullOrWhiteSpace(name))
            {
                return ApplicationErrors.People.NameIsRequired;
            }

            if (!EnsureNameIsAtMost100Characters(name))
            {
                return ApplicationErrors.People.NameMustBeAtMost100Characters;
            }

            if (!EnsureNationalCodeIsValid(nationalCode))
            {
                return ApplicationErrors.People.NationalCodeIsInvalid;
            }

            if (NationalCode != nationalCode && await IsNationalCodeExistAsync(nationalCode, personDomainService, cancellationToken))
            {
                return ApplicationErrors.People.NationalCodeAlreadyExist;
            }

            Name = name;
            NationalCode = nationalCode;

            AddEvent(new PersonChanged(Id, Name, NationalCode));

            return true;
        }

        #region validations

        static bool ValueIsNullOrWhiteSpace(string? value)
        {
            return string.IsNullOrWhiteSpace(value);
        }
        static bool EnsureNameIsAtMost100Characters(string? name)
        {
            return !string.IsNullOrWhiteSpace(name) && name.Length <= 100;
        }
        static bool EnsureNationalCodeIsValid(string? nationalCode)
        {
            return nationalCode.IsValidNationalCode();
        }
        static bool EnsureAddressIsAtMost500Characters(string? name)
        {
            return !string.IsNullOrWhiteSpace(name) && name.Length <= 500;
        }
        static async Task<bool> IsCityExistsAsync(CityId cityId, ICityExistByIdDomainService cityDomainService, CancellationToken cancellationToken = default!)
        {
            return await cityDomainService.IsCityExistAsync(cityId, cancellationToken);
        }
        static async Task<bool> IsProvinceExistAsync(ProvinceId provinceId, IProvinceExistByIdDomainService provinceDomainService, CancellationToken cancellationToken = default!)
        {
            return await provinceDomainService.IsProvinceExistAsync(provinceId, cancellationToken);
        }
        static async Task<bool> IsNationalCodeExistAsync(string? nationalCode, IPersonExistByNationalCodeDomainService personDomainService, CancellationToken cancellationToken = default!)
        {
            return await personDomainService.IsNationalCodeExistAsync(nationalCode, cancellationToken);
        }

        #endregion
    }
}
