using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
//using Application.Intrerfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _repo;
        private readonly IPdfExportService _pdfExportService; // For PDF export

        public AttendanceService(IAttendanceRepository repo, IPdfExportService pdfExportService)
        {
            _repo = repo;
            _pdfExportService = pdfExportService;
        }

        public async Task<List<AttendanceRecord>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<AttendanceRecord?> GetTodayAsync()
        {
            return await _repo.GetTodayAsync();
        }

        public async Task<AttendanceRecord> ToggleCheckAsync()
        {
            // Get today's attendance record (if it exists) along with its sessions.
            var todayRecord = await _repo.GetTodayAsync();
            var now = DateTime.Now;

            if (todayRecord == null)
            {
                // No record for today: create new record and add the first session (clock in)
                todayRecord = new AttendanceRecord
                {
                    Date = DateTime.Today,
                    TotalWorkDuration = TimeSpan.Zero,
                    TotalBreakDuration = TimeSpan.Zero
                };
                // New session
                var newSession = new AttendanceSession { EntryTime = now };
                todayRecord.Sessions.Add(newSession);
                await _repo.AddAsync(todayRecord);
            }
            else
            {
                // Record exists. Check the last session.
                var lastSession = todayRecord.Sessions.OrderBy(s => s.Id).LastOrDefault();

                if (lastSession == null || lastSession.ExitTime != null)
                {
                    // Last session was closed – start a new session (clock in)
                    if (lastSession != null && lastSession.ExitTime.HasValue)
                    {
                        // Calculate break duration between last exit and now
                        var breakDuration = now - lastSession.ExitTime.Value;
                        todayRecord.TotalBreakDuration = (todayRecord.TotalBreakDuration ?? TimeSpan.Zero) + breakDuration;
                    }
                    // Add new session with clock in time
                    var newSession = new AttendanceSession { EntryTime = now };
                    todayRecord.Sessions.Add(newSession);
                }
                else
                {
                    // Last session is open – clock out.
                    lastSession.ExitTime = now;
                    var sessionWorkDuration = lastSession.ExitTime.Value - lastSession.EntryTime.Value;
                    todayRecord.TotalWorkDuration = (todayRecord.TotalWorkDuration ?? TimeSpan.Zero) + sessionWorkDuration;
                }
                await _repo.UpdateAsync(todayRecord);
            }

            await _repo.SaveChangesAsync();
            return todayRecord;
        }

        public async Task<byte[]> ExportMonthlyPdfAsync(int year, int month)
        {
            // Retrieve all records, then filter by year and month.
            var allRecords = await _repo.GetAllAsync();
            var filtered = allRecords
                .Where(r => r.Date.Year == year && r.Date.Month == month)
                .OrderBy(r => r.Date)
                .ToList();

            return _pdfExportService.ExportAttendanceToPdf(filtered, year, month);
        }
    }
}
