using AsanPardakht.Core.Data;
using Newtonsoft.Json;

namespace AsanPardakht.Core.Common
{
    public static class CommonExtensions
    {
        public static bool IsValidNationalCode(this string? nationalCode)
        {
            return !string.IsNullOrWhiteSpace(nationalCode) && nationalCode.Length == 10 && nationalCode.All(char.IsDigit);
        }

        public static string? Serialize<TObject>(this TObject @object) where TObject : notnull
        {
            return JsonConvert.SerializeObject(@object);
        }
        public static object? Deserialize(this string? eventString, Type eventType)
        {
            if (eventString == null)
                return null;

            return JsonConvert.DeserializeObject(eventString, eventType);
        }

        public static OutBoxEventData ToOutboxEventData<TObject>(this TObject @object, string? issuedBy, DateTime issuedAt, string? aggregateType) where TObject : class
        {
            OutBoxEventData outBoxEventData = new()
            {
                IssuedAt = issuedAt,
                IssuedBy = issuedBy,
                AggregateType = aggregateType,
                Data = @object.Serialize(),
                EventType = typeof(TObject).AssemblyQualifiedName,
                AggregateId = @object.GetType().GetProperty("Id")!.GetValue(@object)!.ToString(),
            };

            return outBoxEventData;
        }
    }
}