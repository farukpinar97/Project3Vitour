namespace Project3Vitour.Dtos.TourDtos
{
    public class CreateTourDto
    {
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
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string AdvanceFacilities { get; set; }
        public string WhatToExpect { get; set; }
        public List<string> IncludedServices { get; set; } = new();
        public List<string> ExcludedServices { get; set; } = new();
        public List<string> Amenities { get; set; } = new();
        public List<string> ExpectList { get; set; } = new();
        public List<string> GalleryImages { get; set; } = new();
        public List<TourDayPlanDto> DayPlans { get; set; } = new();
    }
}