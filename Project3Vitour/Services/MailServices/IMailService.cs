namespace Project3Vitour.Services.MailServices
{
    public interface IMailService
    {
        Task SendBookingConfirmationAsync(string toEmail, string toName, string tourTitle, DateTime bookingDate, int participantCount, decimal totalPrice, string bookingNumber);
        Task SendBookingCancellationAsync(string toEmail, string toName, string tourTitle, string bookingNumber);
        Task SendCustomMailAsync(string toEmail, string toName, string subject, string body);
    }
}