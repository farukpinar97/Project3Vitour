namespace Project3Vitour.Dtos.ReviewDtos
{
    public class CreateReviewDto
    {
        public string TourID { get; set; }
        public string AuthorName { get; set; }
        public string AuthorEmail { get; set; }
        public string Comment { get; set; }
        public int ValueRating { get; set; }
        public int DestinationRating { get; set; }
        public int AccommodationRating { get; set; }
        public int TransportRating { get; set; }
        public bool IsApproved { get; set; } = false;
        public string ModerationNote { get; set; }
    }
}