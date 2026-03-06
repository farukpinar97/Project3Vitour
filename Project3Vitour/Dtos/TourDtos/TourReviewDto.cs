namespace Project3Vitour.Dtos.TourDtos
{
    public class TourReviewDto
    {
        public string ReviewID { get; set; }
        public string AuthorName { get; set; }
        public string Comment { get; set; }
        public int ValueRating { get; set; }
        public int DestinationRating { get; set; }
        public int AccommodationRating { get; set; }
        public int TransportRating { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsApproved { get; set; } = false;
        public string ModerationNote { get; set; }
    }
}