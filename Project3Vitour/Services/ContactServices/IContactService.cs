using Project3Vitour.Dtos.ContactDtos;
using Project3Vitour.Entities;

namespace Project3Vitour.Services.ContactServices
{
    public interface IContactService
    {
        Task CreateMessageAsync(CreateContactMessageDto dto);
        Task<List<ContactMessage>> GetAllMessagesAsync();
        Task<ContactMessage> GetMessageByIdAsync(string id);
        Task MarkAsReadAsync(string id);
        Task DeleteMessageAsync(string id);
    }
}