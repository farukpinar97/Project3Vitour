namespace Project3Vitour.Dtos.TourDtos
{
    public class GetTourByIdDto
    {
        public string TourID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CoverImageUrl { get; set; }
        public string Badge { get; set; }
        public int DayCount { get; set; }
        public int Capacity { get; set; }
        public decimal Price { get; set; }
        public bool Status { get; set; }
        public string Location { get; set; }
        public string DepartureLocation { get; set; }
        public string DepartureTime { get; set; }
        public string ReturnTime { get; set; }
        public string AdvanceFacilities { get; set; }
        public string WhatToExpect { get; set; }
        public List<string> IncludedServices { get; set; }
        public List<string> ExcludedServices { get; set; }
        public List<string> Amenities { get; set; }
        public List<string> ExpectList { get; set; }
        public List<TourDayPlanDto> DayPlans { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public List<string> GalleryImages { get; set; }
        public List<TourReviewDto> Reviews { get; set; }
    }
}