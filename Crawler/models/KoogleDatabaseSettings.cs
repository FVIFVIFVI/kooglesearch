namespace KoogleDatabaseSettingsApi.Models
{
    public class KoogleDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string BaseUrlCollectionName { get; set; } = null!;
        public string SpamCollectionName { get; set; } = null!;
        public string UrlsCollectionName { get; set; } = null!;
        public string UsesrCollectionName { get; set; } = null!;
        public string WordsCollectionName { get; set; } = null!;
        public string IgnoreCollectionName { get; set; } = null!;

    }
}
