using System.Globalization;
using System.Text;
using Application.Interfaces;
using Domain.Entities;
using iText;
using System.IO;
using iText.Html2pdf;

namespace Infrastructure.Services
{
    public class PdfExportService : IPdfExportService
    {
        public byte[] ExportAttendanceToPdf(List<AttendanceRecord> records, int year, int month)
        {
            // Generate HTML content for the PDF.
            var html = GenerateHtmlForRecords(records, year, month);

            using (var ms = new MemoryStream())
            {
                // Convert HTML to PDF using iText7's HtmlConverter.
                HtmlConverter.ConvertToPdf(html, ms);
                return ms.ToArray();
            }
        }

        private string GenerateHtmlForRecords(List<AttendanceRecord> records, int year, int month)
        {
            var sb = new StringBuilder();
            sb.Append("<html><head><meta charset='utf-8'><style>");
            sb.Append("body { font-family: Arial, sans-serif; }");
            sb.Append("table { width: 100%; border-collapse: collapse; }");
            sb.Append("th, td { border: 1px solid #ddd; padding: 8px; text-align: center; }");
            sb.Append("th { background-color: #f2f2f2; }");
            sb.Append("</style></head><body>");

            sb.Append($"<h2>Attendance Report for {CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month)} {year}</h2>");

            // Summary table: total work and break durations.
            sb.Append("<h3>Daily Summary</h3>");
            sb.Append("<table>");
            sb.Append("<tr><th>Date</th><th>Total Work Time</th><th>Total Break Time</th></tr>");
            foreach (var r in records)
            {
                sb.Append("<tr>");
                sb.Append($"<td>{r.Date:yyyy-MM-dd}</td>");
                sb.Append($"<td>{(r.TotalWorkDuration.HasValue ? r.TotalWorkDuration.Value.ToString(@"hh\:mm\:ss") : "-")}</td>");
                sb.Append($"<td>{(r.TotalBreakDuration.HasValue ? r.TotalBreakDuration.Value.ToString(@"hh\:mm\:ss") : "-")}</td>");
                sb.Append("</tr>");
            }
            sb.Append("</table>");

            // Detailed sessions
            sb.Append("<h3>Session Details</h3>");
            sb.Append("<table>");
            sb.Append("<tr><th>Date</th><th>Session #</th><th>Check-In</th><th>Check-Out</th><th>Session Duration</th></tr>");
            foreach (var r in records.OrderBy(x => x.Date))
            {
                int sessionCounter = 1;
                foreach (var s in r.Sessions.OrderBy(s => s.Id))
                {
                    sb.Append("<tr>");
                    sb.Append($"<td>{r.Date:yyyy-MM-dd}</td>");
                    sb.Append($"<td>{sessionCounter}</td>");
                    sb.Append($"<td>{(s.EntryTime.HasValue ? s.EntryTime.Value.ToString("HH:mm:ss") : "")}</td>");
                    sb.Append($"<td>{(s.ExitTime.HasValue ? s.ExitTime.Value.ToString("HH:mm:ss") : "")}</td>");
                    var sessionDuration = s.EntryTime.HasValue && s.ExitTime.HasValue
                        ? (s.ExitTime.Value - s.EntryTime.Value).ToString(@"hh\:mm\:ss")
                        : "-";
                    sb.Append($"<td>{sessionDuration}</td>");
                    sb.Append("</tr>");
                    sessionCounter++;
                }
            }
            sb.Append("</table>");

            sb.Append("</body></html>");
            return sb.ToString();
        }
    }
}
