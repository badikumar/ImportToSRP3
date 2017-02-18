namespace ImportToSRP3.Models
{
    public class SrpImporterRequest
    {
        public string NationalCommunity { get; set; }
        public string Region { get; set; }
        public string Cluster { get; set; }
        public string FilePath { get; set; }
    }
}