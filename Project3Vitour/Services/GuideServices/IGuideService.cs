using Project3Vitour.Dtos.GuideDtos;

namespace Project3Vitour.Services.GuideServices
{
    public interface IGuideService
    {
        Task<List<ResultGuideDto>> GetAllGuidesAsync();
        Task<ResultGuideDto> GetGuideByIdAsync(string id);
        Task CreateGuideAsync(CreateGuideDto dto);
        Task UpdateGuideAsync(UpdateGuideDto dto);
        Task DeleteGuideAsync(string id);
        Task<List<ResultGuideDto>> GetAllGuidesForAdminAsync();
    }
}