namespace Project3Vitour.Entities
{
    public class TourDayPlan
    {
        public int DayNumber { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> Highlights { get; set; } = new();
    }
}