namespace Project3Vitour.Dtos.GuideDtos
{
    public class UpdateGuideDto
    {
        public string GuideID { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string LinkedIn { get; set; }
        public string YouTube { get; set; }
        public bool Status { get; set; }
    }
}