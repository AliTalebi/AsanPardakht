namespace AsanPardakht.Core.Errors
{
    public static class ApplicationErrors
    {
        public static Error DataNotFound = new("data not found", 404);
        public static Error DataIsInvalid = new("data is invalid", 400);
        public static Error Unhandled = new("unhandled error occured", 500);

        public static class Province
        {
            public static Error NameIsRequired = new("province, name is required", 400);
            public static Error NameAlreadyTaken = new("province, name already taken", 400);
            public static Error NameMustBeAtMost100Characters = new("province, name must be at most 100 characters", 400);
        }

        public static class City
        {
            public static Error NameIsRequired = new("city, name is required", 400);
            public static Error NameAlreadyTaken = new("city, name already taken", 400);
            public static Error ProvinceNotFound = new("city, province not found", 400);
            public static Error NameMustBeAtMost100Characters = new("city, name must be at most 100 characters", 400);
        }

        public static class People
        {
            public static Error CityNotFound = new("people, city not found", 400);
            public static Error NameIsRequired = new("people, name is required", 400);
            public static Error ProvinceNotFound = new("people, province not found", 400);
            public static Error NationalCodeIsInvalid = new("people, nationalcode is invalid", 400);
            public static Error AddressDetailIsRequired = new("people, address detail is required", 400);
            public static Error NationalCodeAlreadyExist = new("people, nationalcode already exist", 400);
            public static Error NameMustBeAtMost100Characters = new("people, name must be at most 100 characters", 400);
            public static Error AddressDetailMustBeAtMost500Characters = new("people, address detail must be at most 500 characters", 400);
        }
    }
}