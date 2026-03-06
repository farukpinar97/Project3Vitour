namespace Project3Vitour.Dtos.ReviewDtos
{
    public class UpdateReviewDto
    {
        public string ReviewID { get; set; }
        public string NameSurname { get; set; }
        public string Detail { get; set; }
        public int Score { get; set; }
        public DateTime ReviewDate { get; set; }
        public bool Status { get; set; }
        public string TourID { get; set; }
    }
}
