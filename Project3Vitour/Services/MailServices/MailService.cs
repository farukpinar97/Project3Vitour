using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Project3Vitour.Settings;

namespace Project3Vitour.Services.MailServices
{
    public class MailService : IMailService
    {
        private readonly MailSettings _settings;

        public MailService(IOptions<MailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendBookingConfirmationAsync(string toEmail, string toName, string tourTitle, DateTime bookingDate, int participantCount, decimal totalPrice, string bookingNumber)
        {
            var subject = $"Rezervasyonunuz Onaylandı — {tourTitle}";
            var body = $@"
    <div style='font-family:Arial,sans-serif; max-width:600px; margin:0 auto;'>
        <div style='background:#1a7f5a; padding:30px; text-align:center;'>
            <h1 style='color:#fff; margin:0; font-size:24px;'>Vitour</h1>
        </div>
        <div style='padding:40px; background:#f9fafb;'>
            <h2 style='color:#1a7f5a;'>🎉 Rezervasyonunuz Onaylandı!</h2>
            <p>Sayın <strong>{toName}</strong>,</p>
            <p><strong>{tourTitle}</strong> turunuza ait rezervasyonunuz onaylanmıştır.</p>

            <div style='background:#f0fdf4; border:2px dashed #16a34a; border-radius:12px; padding:20px; margin:24px 0; text-align:center;'>
                <p style='font-size:13px; color:#6b7280; margin-bottom:6px;'>Rezervasyon Numaranız</p>
                <p style='font-size:28px; font-weight:800; color:#16a34a; letter-spacing:3px; margin:0;'>{bookingNumber}</p>
                <p style='font-size:12px; color:#6b7280; margin-top:6px;'>Bu numarayı saklayınız</p>
            </div>

            <div style='background:#fff; border-radius:12px; padding:24px; margin:24px 0; border:1px solid #e5e7eb;'>
                <table style='width:100%;'>
                    <tr>
                        <td style='color:#6b7280; padding:8px 0;'>Tur</td>
                        <td style='font-weight:600; text-align:right;'>{tourTitle}</td>
                    </tr>
                    <tr>
                        <td style='color:#6b7280; padding:8px 0;'>Tarih</td>
                        <td style='font-weight:600; text-align:right;'>{bookingDate:dd MMM yyyy}</td>
                    </tr>
                    <tr>
                        <td style='color:#6b7280; padding:8px 0;'>Katılımcı</td>
                        <td style='font-weight:600; text-align:right;'>{participantCount} Kişi</td>
                    </tr>
                    <tr style='border-top:1px solid #e5e7eb;'>
                        <td style='font-weight:700; padding:12px 0 0;'>Toplam</td>
                        <td style='font-weight:800; font-size:18px; color:#1a7f5a; text-align:right; padding-top:12px;'>{totalPrice:N0} ₺</td>
                    </tr>
                </table>
            </div>
            <p style='color:#6b7280;'>Herhangi bir sorunuz için bizimle iletişime geçebilirsiniz.</p>
        </div>
        <div style='background:#1a7f5a; padding:20px; text-align:center;'>
            <p style='color:#fff; margin:0; font-size:13px;'>© 2025 Vitour. Tüm hakları saklıdır.</p>
        </div>
    </div>";

            await SendAsync(toEmail, toName, subject, body);
        }

        public async Task SendBookingCancellationAsync(string toEmail, string toName, string tourTitle, string bookingNumber)
        {
            var subject = $"Rezervasyonunuz İptal Edildi — {tourTitle}";
            var body = $@"
    <div style='font-family:Arial,sans-serif; max-width:600px; margin:0 auto;'>
        <div style='background:#1a7f5a; padding:30px; text-align:center;'>
            <h1 style='color:#fff; margin:0; font-size:24px;'>Vitour</h1>
        </div>
        <div style='padding:40px; background:#f9fafb;'>
            <h2 style='color:#ef4444;'>❌ Rezervasyonunuz İptal Edildi</h2>
            <p>Sayın <strong>{toName}</strong>,</p>
            <p><strong>{tourTitle}</strong> turunuza ait <strong>{bookingNumber}</strong> numaralı rezervasyonunuz maalesef iptal edilmiştir.</p>
            <div style='background:#fee2e2; border-radius:12px; padding:20px; margin:24px 0;'>
                <p style='color:#ef4444; margin:0;'>Daha fazla bilgi almak için lütfen bizimle iletişime geçin.</p>
            </div>
            <a href='https://localhost:7196/Tour/TourList'
               style='display:inline-block; padding:12px 24px; background:#1a7f5a; color:#fff; border-radius:8px; text-decoration:none; font-weight:600;'>
                Diğer Turlara Bak
            </a>
        </div>
        <div style='background:#1a7f5a; padding:20px; text-align:center;'>
            <p style='color:#fff; margin:0; font-size:13px;'>© 2025 Vitour. Tüm hakları saklıdır.</p>
        </div>
    </div>";

            await SendAsync(toEmail, toName, subject, body);
        }

        public async Task SendCustomMailAsync(string toEmail, string toName, string subject, string body)
        {
            await SendAsync(toEmail, toName, subject, $"<p>{body}</p>");
        }

        private async Task SendAsync(string toEmail, string toName, string subject, string htmlBody)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            message.To.Add(new MailboxAddress(toName, toEmail));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = htmlBody };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_settings.SenderEmail, _settings.Password);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
    }
}