namespace Project3Vitour.Settings
{
    public interface IDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string TourCollectionName { get; set; }
        public string CategoryCollectionName { get; set; }
        string ReviewCollectionName { get; set; } // YENİ
        string ContactCollectionName { get; set; }

        string GuideCollectionName { get; set; }

        public string BookingCollectionName { get; set; }
    }
}
