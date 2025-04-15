using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IAttendanceService
    {
        Task<List<AttendanceRecord>> GetAllAsync();
        Task<AttendanceRecord?> GetTodayAsync();
        Task<AttendanceRecord> ToggleCheckAsync();
        Task<byte[]> ExportMonthlyPdfAsync(int year, int month);
    }
}
