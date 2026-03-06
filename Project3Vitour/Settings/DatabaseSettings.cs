namespace Project3Vitour.Settings
{
    public class DatabaseSettings:IDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string TourCollectionName { get; set; }
        public string CategoryCollectionName { get; set; }
        public string ReviewCollectionName { get; set; }
        public string ContactCollectionName { get; set; }

        public string GuideCollectionName { get; set; }

        public string BookingCollectionName { get; set; }
    }
}
