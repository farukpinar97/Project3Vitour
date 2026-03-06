using AutoMapper;
using MongoDB.Driver;
using Project3Vitour.Dtos.TourDtos;
using Project3Vitour.Entities;
using Project3Vitour.Settings;

namespace Project3Vitour.Services.TourServices
{
    public class TourService : ITourService
    {
        private readonly IMapper _mapper;
        private readonly IMongoCollection<Tour> _tourCollection;

        public TourService(IMapper mapper,IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _tourCollection = database.GetCollection<Tour>(_databaseSettings.TourCollectionName);
            _mapper = mapper;
        }

        public async Task CreateTourAsync(CreateTourDto createTourDto)
        {
            var value =  _mapper.Map<Tour>(createTourDto);
            await _tourCollection.InsertOneAsync(value);
        }

        public async Task DeleteTourAsync(string id)
        {
            await _tourCollection.DeleteOneAsync(x=>x.TourID == id);
        }

        public async Task<List<ResultTourDto>> GetAllTourAsync()
        {
            var values = await _tourCollection.Find(x=>true).ToListAsync();
            return _mapper.Map<List<ResultTourDto>>(values);
        }

        public async Task<GetTourByIdDto> GetTourByIdAsync(string id)
        {
            var value = await _tourCollection.Find(x=> x.TourID==id).FirstOrDefaultAsync();
            return _mapper.Map<GetTourByIdDto>(value);
        }

        public async Task UpdateTourAsync(UpdateTourDto updateTourDto)
        {
            var values = _mapper.Map<Tour>(updateTourDto);
            await _tourCollection.FindOneAndReplaceAsync(x=>x.TourID == updateTourDto.TourID,values);
        }

        public async Task<(List<ResultTourDto> Tours, long TotalCount)> GetPagedToursAsync(int page, int pageSize)
        {
            var totalCount = await _tourCollection.CountDocumentsAsync(x => true);
            var values = await _tourCollection.Find(x => true)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
            return (_mapper.Map<List<ResultTourDto>>(values), totalCount);
        }

        public async Task<List<ResultTourDto>> GetFeaturedToursAsync(int count = 5)
        {
            var tours = await _tourCollection
                .Find(x => x.Status == true)
                .ToListAsync();

            var random = tours.OrderBy(_ => Guid.NewGuid()).Take(count).ToList();
            return _mapper.Map<List<ResultTourDto>>(random);
        }
    }
}
