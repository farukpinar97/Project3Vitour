using Microsoft.AspNetCore.Mvc;
using Project3Vitour.Services.BookingServices;
using Project3Vitour.Services.MailServices;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Project3Vitour.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminBookingController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly IMailService _mailService;

        public AdminBookingController(IBookingService bookingService, IMailService mailService)
        {
            _bookingService = bookingService;
            _mailService = mailService;
        }

        public async Task<IActionResult> Index(int page = 1, string filter = "all")
        {
            const int pageSize = 10;
            ViewBag.Title = "Rezervasyonlar";
            ViewBag.ActiveMenu = "bookings";

            var allBookings = await _bookingService.GetAllBookingsAsync();

            ViewBag.TotalCount = allBookings.Count;
            ViewBag.PendingCount = allBookings.Count(x => x.Status == "Pending");
            ViewBag.ConfirmedCount = allBookings.Count(x => x.Status == "Confirmed");
            ViewBag.CancelledCount = allBookings.Count(x => x.Status == "Cancelled");

            var filtered = filter switch
            {
                "pending" => allBookings.Where(x => x.Status == "Pending").ToList(),
                "confirmed" => allBookings.Where(x => x.Status == "Confirmed").ToList(),
                "cancelled" => allBookings.Where(x => x.Status == "Cancelled").ToList(),
                _ => allBookings
            };

            var paged = filtered.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(filtered.Count / (double)pageSize);
            ViewBag.CurrentFilter = filter;

            return View(paged);
        }

        public async Task<IActionResult> Detail(string id)
        {
            ViewBag.Title = "Rezervasyon Detayı";
            ViewBag.ActiveMenu = "bookings";
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null) return NotFound();
            return View(booking);
        }

        public async Task<IActionResult> Confirm(string id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            await _bookingService.UpdateStatusAsync(id, "Confirmed");
            try
            {
                await _mailService.SendBookingConfirmationAsync(
                    booking.Email, booking.FullName, booking.TourTitle,
                    booking.BookingDate, booking.ParticipantCount, booking.TotalPrice,
                    booking.BookingNumber);
                TempData["Success"] = "Rezervasyon onaylandı ve mail gönderildi!";
            }
            catch
            {
                TempData["Success"] = "Rezervasyon onaylandı fakat mail gönderilemedi.";
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Cancel(string id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            await _bookingService.UpdateStatusAsync(id, "Cancelled");
            try
            {
                await _mailService.SendBookingCancellationAsync(
                    booking.Email, booking.FullName, booking.TourTitle,
                    booking.BookingNumber);
                TempData["Success"] = "Rezervasyon iptal edildi ve mail gönderildi!";
            }
            catch
            {
                TempData["Success"] = "Rezervasyon iptal edildi fakat mail gönderilemedi.";
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _bookingService.DeleteBookingAsync(id);
            TempData["Success"] = "Rezervasyon silindi!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> SendEmail(string id)
        {
            ViewBag.Title = "Email Gönder";
            ViewBag.ActiveMenu = "bookings";
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null) return NotFound();
            return View(booking);
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(string id, string subject, string body)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            try
            {
                await _mailService.SendCustomMailAsync(booking.Email, booking.FullName, subject, body);
                TempData["Success"] = "Mail başarıyla gönderildi!";
            }
            catch
            {
                TempData["Error"] = "Mail gönderilemedi, lütfen tekrar deneyin.";
            }
            return RedirectToAction("Detail", new { id });
        }

        public async Task<IActionResult> ExportPdf(string tourId)
        {
            var bookings = await _bookingService.GetBookingsByTourIdAsync(tourId);
            var tourTitle = bookings.FirstOrDefault()?.TourTitle ?? "Tur";

            using var ms = new MemoryStream();
            var writer = new PdfWriter(ms);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf, iText.Kernel.Geom.PageSize.A4.Rotate());

            var font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            var fontBold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            var green = new DeviceRgb(26, 127, 90);
            var lightGreen = new DeviceRgb(240, 250, 245);
            var gray = new DeviceRgb(107, 114, 128);

            document.Add(new Paragraph("VITOUR")
                .SetFont(fontBold).SetFontSize(24).SetFontColor(green)
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

            document.Add(new Paragraph($"{tourTitle} - Katilimci Listesi")
                .SetFont(fontBold).SetFontSize(14)
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetMarginBottom(4));

            document.Add(new Paragraph(
                $"Olusturulma: {DateTime.Now:dd MMM yyyy HH:mm}  |  Toplam: {bookings.Count} Rezervasyon")
                .SetFont(font).SetFontSize(9).SetFontColor(gray)
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetMarginBottom(20));

            // Rezervasyon No eklendi
            var table = new Table(new float[] { 0.5f, 2f, 2.5f, 2.5f, 2f, 2f, 1.5f, 2f })
                .UseAllAvailableWidth();

            var headers = new[] { "#", "Rezervasyon No", "Ad Soyad", "E-posta", "Telefon", "Tur Tarihi", "Kisi", "Toplam" };
            foreach (var header in headers)
            {
                table.AddHeaderCell(new Cell()
                    .Add(new Paragraph(header).SetFont(fontBold).SetFontSize(9)
                        .SetFontColor(ColorConstants.WHITE))
                    .SetBackgroundColor(green).SetPadding(8));
            }

            for (int i = 0; i < bookings.Count; i++)
            {
                var b = bookings[i];
                var bg = i % 2 == 0 ? ColorConstants.WHITE : lightGreen;
                var cells = new[]
                {
                    (i + 1).ToString(),
                    b.BookingNumber ?? "-",
                    b.FullName, b.Email, b.Phone,
                    b.BookingDate.ToString("dd MMM yyyy"),
                    $"{b.ParticipantCount} Kisi",
                    $"{b.TotalPrice:N0} TL"
                };

                foreach (var cell in cells)
                {
                    table.AddCell(new Cell()
                        .Add(new Paragraph(cell).SetFont(font).SetFontSize(9))
                        .SetBackgroundColor(bg).SetPadding(7));
                }
            }

            document.Add(table);

            document.Add(new Paragraph(
                $"\nToplam Katilimci: {bookings.Sum(x => x.ParticipantCount)} Kisi  |  " +
                $"Toplam Gelir: {bookings.Sum(x => x.TotalPrice):N0} TL")
                .SetFont(fontBold).SetFontSize(11).SetFontColor(green).SetMarginTop(16));

            document.Close();

            return File(ms.ToArray(), "application/pdf",
                $"{tourTitle}_katilimci_{DateTime.Now:yyyyMMdd}.pdf");
        }

        public async Task<IActionResult> ExportExcel(string tourId)
        {
            var bookings = await _bookingService.GetBookingsByTourIdAsync(tourId);
            var tourTitle = bookings.FirstOrDefault()?.TourTitle ?? "Tur";

            ExcelPackage.License.SetNonCommercialPersonal("Vitour");

            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("Katilimcilar");

            // Rezervasyon No eklendi → H sütununa kadar genişledi
            ws.Cells["A1:H1"].Merge = true;
            ws.Cells["A1"].Value = $"{tourTitle} - Katilimci Listesi";
            ws.Cells["A1"].Style.Font.Size = 16;
            ws.Cells["A1"].Style.Font.Bold = true;
            ws.Cells["A1"].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(26, 127, 90));
            ws.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Row(1).Height = 30;

            ws.Cells["A2:H2"].Merge = true;
            ws.Cells["A2"].Value = $"Olusturulma: {DateTime.Now:dd MMM yyyy HH:mm}  |  Toplam: {bookings.Count} Rezervasyon";
            ws.Cells["A2"].Style.Font.Size = 9;
            ws.Cells["A2"].Style.Font.Color.SetColor(System.Drawing.Color.Gray);
            ws.Cells["A2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Row(2).Height = 20;

            var headers = new[] { "#", "Rezervasyon No", "Ad Soyad", "E-posta", "Telefon", "Tur Tarihi", "Kisi Sayisi", "Toplam" };
            for (int i = 0; i < headers.Length; i++)
            {
                var cell = ws.Cells[4, i + 1];
                cell.Value = headers[i];
                cell.Style.Font.Bold = true;
                cell.Style.Font.Color.SetColor(System.Drawing.Color.White);
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(26, 127, 90));
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }
            ws.Row(4).Height = 22;

            for (int i = 0; i < bookings.Count; i++)
            {
                var b = bookings[i];
                var row = i + 5;
                var rowBg = i % 2 == 0
                    ? System.Drawing.Color.White
                    : System.Drawing.Color.FromArgb(240, 250, 245);

                var values = new object[]
                {
                    i + 1,
                    b.BookingNumber ?? "-",
                    b.FullName, b.Email, b.Phone,
                    b.BookingDate.ToString("dd MMM yyyy"),
                    b.ParticipantCount, b.TotalPrice
                };

                for (int j = 0; j < values.Length; j++)
                {
                    var cell = ws.Cells[row, j + 1];
                    cell.Value = values[j];
                    cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    cell.Style.Fill.BackgroundColor.SetColor(rowBg);
                    cell.Style.Border.BorderAround(ExcelBorderStyle.Thin,
                        System.Drawing.Color.FromArgb(229, 231, 235));

                    if (j == 7)
                    {
                        cell.Style.Numberformat.Format = "#,##0 \"TL\"";
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(26, 127, 90));
                    }
                }
                ws.Row(row).Height = 20;
            }

            var summaryRow = bookings.Count + 5;
            ws.Cells[summaryRow, 1, summaryRow, 6].Merge = true;
            ws.Cells[summaryRow, 1].Value = "TOPLAM";
            ws.Cells[summaryRow, 7].Value = bookings.Sum(x => x.ParticipantCount);
            ws.Cells[summaryRow, 8].Value = (double)bookings.Sum(x => x.TotalPrice);
            ws.Cells[summaryRow, 8].Style.Numberformat.Format = "#,##0 \"TL\"";

            for (int j = 1; j <= 8; j++)
            {
                ws.Cells[summaryRow, j].Style.Font.Bold = true;
                ws.Cells[summaryRow, j].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[summaryRow, j].Style.Fill.BackgroundColor
                    .SetColor(System.Drawing.Color.FromArgb(240, 250, 245));
                ws.Cells[summaryRow, j].Style.Border.BorderAround(ExcelBorderStyle.Medium,
                    System.Drawing.Color.FromArgb(26, 127, 90));
                ws.Cells[summaryRow, j].Style.Font.Color
                    .SetColor(System.Drawing.Color.FromArgb(26, 127, 90));
            }
            ws.Row(summaryRow).Height = 22;

            ws.Column(1).Width = 5;
            ws.Column(2).Width = 16;
            ws.Column(3).Width = 22;
            ws.Column(4).Width = 28;
            ws.Column(5).Width = 16;
            ws.Column(6).Width = 16;
            ws.Column(7).Width = 12;
            ws.Column(8).Width = 14;

            var bytes = package.GetAsByteArray();
            return File(bytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"{tourTitle}_katilimci_{DateTime.Now:yyyyMMdd}.xlsx");
        }

        public async Task<IActionResult> ExportAllPdf()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();

            using var ms = new MemoryStream();
            var writer = new PdfWriter(ms);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf, iText.Kernel.Geom.PageSize.A4.Rotate());

            var font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            var fontBold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            var green = new DeviceRgb(26, 127, 90);
            var lightGreen = new DeviceRgb(240, 250, 245);
            var gray = new DeviceRgb(107, 114, 128);

            document.Add(new Paragraph("VITOUR")
                .SetFont(fontBold).SetFontSize(24).SetFontColor(green)
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

            document.Add(new Paragraph("Tum Rezervasyonlar")
                .SetFont(fontBold).SetFontSize(14)
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetMarginBottom(4));

            document.Add(new Paragraph(
                $"Olusturulma: {DateTime.Now:dd MMM yyyy HH:mm}  |  Toplam: {bookings.Count} Rezervasyon")
                .SetFont(font).SetFontSize(9).SetFontColor(gray)
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetMarginBottom(20));

            var table = new Table(new float[] { 0.5f, 2f, 2f, 2.5f, 2f, 2f, 1.5f, 1.5f, 1.5f })
                .UseAllAvailableWidth();

            var headers = new[] { "#", "Rezervasyon No", "Ad Soyad", "E-posta", "Tur", "Tarih", "Kisi", "Toplam", "Durum" };
            foreach (var header in headers)
            {
                table.AddHeaderCell(new Cell()
                    .Add(new Paragraph(header).SetFont(fontBold).SetFontSize(9)
                        .SetFontColor(ColorConstants.WHITE))
                    .SetBackgroundColor(green).SetPadding(8));
            }

            for (int i = 0; i < bookings.Count; i++)
            {
                var b = bookings[i];
                var bg = i % 2 == 0 ? ColorConstants.WHITE : lightGreen;
                var statusLabel = b.Status switch
                {
                    "Confirmed" => "Onaylandi",
                    "Cancelled" => "Iptal",
                    _ => "Bekliyor"
                };
                var cells = new[]
                {
            (i + 1).ToString(),
            b.BookingNumber ?? "-",
            b.FullName, b.Email,
            b.TourTitle,
            b.BookingDate.ToString("dd MMM yyyy"),
            $"{b.ParticipantCount} Kisi",
            $"{b.TotalPrice:N0} TL",
            statusLabel
        };

                foreach (var cell in cells)
                {
                    table.AddCell(new Cell()
                        .Add(new Paragraph(cell).SetFont(font).SetFontSize(9))
                        .SetBackgroundColor(bg).SetPadding(7));
                }
            }

            document.Add(table);

            document.Add(new Paragraph(
                $"\nToplam Katilimci: {bookings.Sum(x => x.ParticipantCount)} Kisi  |  " +
                $"Toplam Gelir: {bookings.Sum(x => x.TotalPrice):N0} TL")
                .SetFont(fontBold).SetFontSize(11).SetFontColor(green).SetMarginTop(16));

            document.Close();

            return File(ms.ToArray(), "application/pdf",
                $"tum_rezervasyonlar_{DateTime.Now:yyyyMMdd}.pdf");
        }

        public async Task<IActionResult> ExportAllExcel()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();

            ExcelPackage.License.SetNonCommercialPersonal("Vitour");

            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("Tum Rezervasyonlar");

            ws.Cells["A1:I1"].Merge = true;
            ws.Cells["A1"].Value = "Tum Rezervasyonlar";
            ws.Cells["A1"].Style.Font.Size = 16;
            ws.Cells["A1"].Style.Font.Bold = true;
            ws.Cells["A1"].Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(26, 127, 90));
            ws.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Row(1).Height = 30;

            ws.Cells["A2:I2"].Merge = true;
            ws.Cells["A2"].Value = $"Olusturulma: {DateTime.Now:dd MMM yyyy HH:mm}  |  Toplam: {bookings.Count} Rezervasyon";
            ws.Cells["A2"].Style.Font.Size = 9;
            ws.Cells["A2"].Style.Font.Color.SetColor(System.Drawing.Color.Gray);
            ws.Cells["A2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Row(2).Height = 20;

            var headers = new[] { "#", "Rezervasyon No", "Ad Soyad", "E-posta", "Tur", "Tarih", "Kisi Sayisi", "Toplam", "Durum" };
            for (int i = 0; i < headers.Length; i++)
            {
                var cell = ws.Cells[4, i + 1];
                cell.Value = headers[i];
                cell.Style.Font.Bold = true;
                cell.Style.Font.Color.SetColor(System.Drawing.Color.White);
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(26, 127, 90));
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }
            ws.Row(4).Height = 22;

            for (int i = 0; i < bookings.Count; i++)
            {
                var b = bookings[i];
                var row = i + 5;
                var rowBg = i % 2 == 0
                    ? System.Drawing.Color.White
                    : System.Drawing.Color.FromArgb(240, 250, 245);

                var statusLabel = b.Status switch
                {
                    "Confirmed" => "Onaylandi",
                    "Cancelled" => "Iptal",
                    _ => "Bekliyor"
                };

                var values = new object[]
                {
            i + 1,
            b.BookingNumber ?? "-",
            b.FullName, b.Email,
            b.TourTitle,
            b.BookingDate.ToString("dd MMM yyyy"),
            b.ParticipantCount,
            b.TotalPrice,
            statusLabel
                };

                for (int j = 0; j < values.Length; j++)
                {
                    var cell = ws.Cells[row, j + 1];
                    cell.Value = values[j];
                    cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    cell.Style.Fill.BackgroundColor.SetColor(rowBg);
                    cell.Style.Border.BorderAround(ExcelBorderStyle.Thin,
                        System.Drawing.Color.FromArgb(229, 231, 235));

                    if (j == 7)
                    {
                        cell.Style.Numberformat.Format = "#,##0 \"TL\"";
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(26, 127, 90));
                    }
                }
                ws.Row(row).Height = 20;
            }

            var summaryRow = bookings.Count + 5;
            ws.Cells[summaryRow, 1, summaryRow, 7].Merge = true;
            ws.Cells[summaryRow, 1].Value = "TOPLAM";
            ws.Cells[summaryRow, 8].Value = (double)bookings.Sum(x => x.TotalPrice);
            ws.Cells[summaryRow, 8].Style.Numberformat.Format = "#,##0 \"TL\"";

            for (int j = 1; j <= 9; j++)
            {
                ws.Cells[summaryRow, j].Style.Font.Bold = true;
                ws.Cells[summaryRow, j].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[summaryRow, j].Style.Fill.BackgroundColor
                    .SetColor(System.Drawing.Color.FromArgb(240, 250, 245));
                ws.Cells[summaryRow, j].Style.Border.BorderAround(ExcelBorderStyle.Medium,
                    System.Drawing.Color.FromArgb(26, 127, 90));
                ws.Cells[summaryRow, j].Style.Font.Color
                    .SetColor(System.Drawing.Color.FromArgb(26, 127, 90));
            }
            ws.Row(summaryRow).Height = 22;

            ws.Column(1).Width = 5;
            ws.Column(2).Width = 16;
            ws.Column(3).Width = 20;
            ws.Column(4).Width = 26;
            ws.Column(5).Width = 22;
            ws.Column(6).Width = 14;
            ws.Column(7).Width = 12;
            ws.Column(8).Width = 14;
            ws.Column(9).Width = 12;

            var bytes = package.GetAsByteArray();
            return File(bytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"tum_rezervasyonlar_{DateTime.Now:yyyyMMdd}.xlsx");
        }
    }
}