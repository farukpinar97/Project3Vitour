using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Project3Vitour.Entities
{
    public class TourReview
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ReviewID { get; set; }
        public string TourID { get; set; }
        public string AuthorName { get; set; }
        public string AuthorEmail { get; set; }
        public string Comment { get; set; }
        public int ValueRating { get; set; }      // 1-5
        public int DestinationRating { get; set; }
        public int AccommodationRating { get; set; }
        public int TransportRating { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsApproved { get; set; } = false;
        public string ModerationNote { get; set; }
    }
}