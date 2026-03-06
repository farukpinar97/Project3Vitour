using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Project3Vitour.Entities
{
    public class Guide
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string GuideID { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }        // Ör: "Kıdemli Tur Rehberi"
        public string ImageUrl { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string LinkedIn { get; set; }
        public string YouTube { get; set; }
        public bool Status { get; set; } = true;
    }
}