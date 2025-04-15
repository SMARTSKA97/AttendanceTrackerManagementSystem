using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IAttendanceRepository
    {
        Task<List<AttendanceRecord>> GetAllAsync();
        Task<AttendanceRecord?> GetTodayAsync();
        Task AddAsync(AttendanceRecord record);
        Task UpdateAsync(AttendanceRecord record);
        Task SaveChangesAsync();
    }
}
