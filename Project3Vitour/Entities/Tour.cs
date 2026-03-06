using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Project3Vitour.Entities
{
    public class Tour
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string TourID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CoverImageUrl { get; set; }
        public string Badge { get; set; }
        public int DayCount { get; set; }
        public int Capacity { get; set; }
        public decimal Price { get; set; }
        public bool Status { get; set; }

        // Bilgi sekmesi
        public string Location { get; set; }           // Konum adresi
        public string DepartureLocation { get; set; }  // Kalkış noktası
        public string DepartureTime { get; set; }      // Kalkış saati (ör: "09:30")
        public string ReturnTime { get; set; }         // Dönüş saati
        public string AdvanceFacilities { get; set; }  // Gelişmiş olanaklar
        public string WhatToExpect { get; set; }       // Ne beklenmeli
        public List<string> IncludedServices { get; set; } = new(); // Dahil hizmetler
        public List<string> ExcludedServices { get; set; } = new(); // Hariç hizmetler
        public List<string> Amenities { get; set; } = new();        // Tur olanakları
        public List<string> ExpectList { get; set; } = new();       // Beklenti listesi

        // Tur planı sekmesi
        public List<TourDayPlan> DayPlans { get; set; } = new();

        // Lokasyon sekmesi
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        // Galeri sekmesi
        public List<string> GalleryImages { get; set; } = new();
    }
}
