using Project3Vitour.Dtos.TourDtos;

namespace Project3Vitour.Services.TourServices
{
    public interface ITourService
    {
        Task<List<ResultTourDto>> GetAllTourAsync();
        Task CreateTourAsync(CreateTourDto createTourDto);
        Task UpdateTourAsync(UpdateTourDto updateTourDto);
        Task DeleteTourAsync(string id);
        Task<GetTourByIdDto> GetTourByIdAsync(string id);

        Task<(List<ResultTourDto> Tours, long TotalCount)> GetPagedToursAsync(int page, int pageSize);

        Task<List<ResultTourDto>> GetFeaturedToursAsync(int count = 5);

    }
}
