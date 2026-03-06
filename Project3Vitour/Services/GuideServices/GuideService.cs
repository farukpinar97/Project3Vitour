using AutoMapper;
using MongoDB.Driver;
using Project3Vitour.Dtos.GuideDtos;
using Project3Vitour.Entities;
using Project3Vitour.Settings;

namespace Project3Vitour.Services.GuideServices
{
    public class GuideService : IGuideService
    {
        private readonly IMongoCollection<Guide> _collection;
        private readonly IMapper _mapper;

        public GuideService(IMapper mapper, IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<Guide>(settings.GuideCollectionName);
            _mapper = mapper;
        }

        public async Task<List<ResultGuideDto>> GetAllGuidesAsync()
        {
            var values = await _collection.Find(x => x.Status == true).ToListAsync();
            return _mapper.Map<List<ResultGuideDto>>(values);
        }

        public async Task<ResultGuideDto> GetGuideByIdAsync(string id)
        {
            var value = await _collection.Find(x => x.GuideID == id).FirstOrDefaultAsync();
            return _mapper.Map<ResultGuideDto>(value);
        }

        public async Task CreateGuideAsync(CreateGuideDto dto)
        {
            var value = _mapper.Map<Guide>(dto);
            await _collection.InsertOneAsync(value);
        }

        public async Task UpdateGuideAsync(UpdateGuideDto dto)
        {
            var value = _mapper.Map<Guide>(dto);
            await _collection.FindOneAndReplaceAsync(x => x.GuideID == dto.GuideID, value);
        }

        public async Task DeleteGuideAsync(string id)
        {
            await _collection.DeleteOneAsync(x => x.GuideID == id);
        }

        public async Task<List<ResultGuideDto>> GetAllGuidesForAdminAsync()
        {
            var values = await _collection.Find(_ => true).ToListAsync();
            return _mapper.Map<List<ResultGuideDto>>(values);
        }
    }
}