namespace Project3Vitour.Dtos.TourDtos
{
    public class TourDayPlanDto
    {
        public int DayNumber { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> Highlights { get; set; }
    }
}