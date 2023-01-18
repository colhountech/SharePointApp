namespace GetSharePointList
{
    public class AzureAD
    {
        public string ClientID { get; set; } = default!;
        public string TenantID { get; set; } = default!;
        public string Username {  get; set; } = default!;
        public string Site { get; set; } = default!;
        public string List { get; set; } = default!;
    }
}