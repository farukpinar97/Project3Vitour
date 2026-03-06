using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Project3Vitour.Entities
{
    public class Booking
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string BookingID { get; set; }
        public string BookingNumber { get; set; }
        public string TourID { get; set; }
        public string TourTitle { get; set; }
        public string TourCoverImageUrl { get; set; }
        public string TourLocation { get; set; }
        public int TourDayCount { get; set; }
        public decimal TourPrice { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int ParticipantCount { get; set; } = 1;
        public DateTime BookingDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Cancelled
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}