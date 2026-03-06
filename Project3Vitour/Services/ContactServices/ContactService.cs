using AutoMapper;
using MongoDB.Driver;
using Project3Vitour.Dtos.ContactDtos;
using Project3Vitour.Entities;
using Project3Vitour.Settings;

namespace Project3Vitour.Services.ContactServices
{
    public class ContactService : IContactService
    {
        private readonly IMongoCollection<ContactMessage> _collection;
        private readonly IMapper _mapper;

        public ContactService(IMapper mapper, IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<ContactMessage>(settings.ContactCollectionName);
            _mapper = mapper;
        }

        public async Task CreateMessageAsync(CreateContactMessageDto dto)
        {
            var message = _mapper.Map<ContactMessage>(dto);
            await _collection.InsertOneAsync(message);
        }

        public async Task<List<ContactMessage>> GetAllMessagesAsync()
        {
            return await _collection
                .Find(_ => true)
                .SortByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<ContactMessage> GetMessageByIdAsync(string id)
        {
            return await _collection
                .Find(x => x.MessageID == id)
                .FirstOrDefaultAsync();
        }

        public async Task MarkAsReadAsync(string id)
        {
            var update = Builders<ContactMessage>.Update.Set(x => x.IsRead, true);
            await _collection.UpdateOneAsync(x => x.MessageID == id, update);
        }

        public async Task DeleteMessageAsync(string id)
        {
            await _collection.DeleteOneAsync(x => x.MessageID == id);
        }
    }
}