namespace AsanPardakht.WorkerService.MessageProcessorConsoleApp.Data.Entities
{
    public sealed class ProcessedTehranCityAddress
    {
        public int Id { get; set; }
        public string? CityName { get; set; }
        public string? ProvinceName { get; set; }
        public string? Address { get; set; }
        public int? AddressId { get; set; }
    }
}