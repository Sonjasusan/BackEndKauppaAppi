namespace RuokaAppiBackend.Models
{
    public class KauppaOperation
    {
        public int KavijaID { get; set; }
        public int KauppaOstosID { get; set; }
        public string? OperationType { get; set; }
        public string? Comment { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
    }
}
