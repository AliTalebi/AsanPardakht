namespace AsanPardakht.WorkerService.MessageProcessorConsoleApp.Outbox.Http.DTO
{
    public sealed class PeopleAddressHttpResponse
    {
        public int TotalCount { get; set; }
        public List<PeopleAddressResult> Data { get; set; } = new();
    }

    public sealed class PeopleAddressResult
    {
        public int Id { get; set; }
        public string? Detail { get; set; }

        public PeopleAddressCityResult? City { get; set; }
        public PeopleAddressProvinceResult? Province { get; set; }
    }

    public record PeopleAddressCityResult
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    public record PeopleAddressProvinceResult
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
