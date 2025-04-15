using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Models; // scaffolded EF models
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly AttendanceDbContext _context;

        public AttendanceRepository(AttendanceDbContext context)
        {
            _context = context;
        }

        // Mapping from EF model (Infrastructure.Models.AttendanceRecord) to Domain model (Domain.Entities.AttendanceRecord)
        private Domain.Entities.AttendanceRecord MapToDomain(Infrastructure.Models.AttendanceRecord efRecord)
        {
            return new Domain.Entities.AttendanceRecord
            {
                Id = efRecord.Id,
                // Convert DateOnly (EF model) to DateTime (Domain model). Assume time part is midnight.
                Date = efRecord.Date.ToDateTime(TimeOnly.MinValue),
                TotalWorkDuration = efRecord.TotalWorkDuration,
                TotalBreakDuration = efRecord.TotalBreakDuration,
                // If UserId is 0, treat as null (adjust as needed)
                UserId = (efRecord.UserId == 0) ? null : efRecord.UserId,
                Sessions = efRecord.AttendanceSessions?.Select(s => new Domain.Entities.AttendanceSession
                {
                    Id = s.Id,
                    EntryTime = s.EntryTime,     // assuming EntryTime is already DateTime? in the EF model
                    ExitTime = s.ExitTime,       // same assumption for ExitTime
                    AttendanceRecordId = s.AttendanceRecordId
                }).ToList() ?? new List<Domain.Entities.AttendanceSession>()
            };
        }

        // Mapping from Domain model to EF model.
        private Infrastructure.Models.AttendanceRecord MapToInfrastructure(Domain.Entities.AttendanceRecord domainRecord)
        {
            return new Infrastructure.Models.AttendanceRecord
            {
                Id = domainRecord.Id,
                // Convert DateTime to DateOnly (store only the date part)
                Date = DateOnly.FromDateTime(domainRecord.Date),
                TotalWorkDuration = domainRecord.TotalWorkDuration,
                TotalBreakDuration = domainRecord.TotalBreakDuration,
                UserId = domainRecord.UserId ?? 0,
                AttendanceSessions = domainRecord.Sessions.Select(s => new Infrastructure.Models.AttendanceSession
                {
                    Id = s.Id,
                    // Assume the EF model stores EntryTime and ExitTime as DateTime? directly.
                    EntryTime = s.EntryTime,
                    ExitTime = s.ExitTime,
                    AttendanceRecordId = s.AttendanceRecordId
                }).ToList()
            };
        }

        public async Task<List<Domain.Entities.AttendanceRecord>> GetAllAsync()
        {
            var efRecords = await _context.AttendanceRecords
                .Include(r => r.AttendanceSessions)
                .ToListAsync();

            return efRecords.Select(MapToDomain).ToList();
        }

        public async Task<Domain.Entities.AttendanceRecord?> GetTodayAsync()
        {
            // Convert today's date to DateOnly.
            var today = DateOnly.FromDateTime(DateTime.Today);
            var efRecord = await _context.AttendanceRecords
                .Include(r => r.AttendanceSessions)
                .FirstOrDefaultAsync(r => r.Date == today);

            return efRecord == null ? null : MapToDomain(efRecord);
        }

        public async Task AddAsync(Domain.Entities.AttendanceRecord domainRecord)
        {
            var efRecord = MapToInfrastructure(domainRecord);
            _context.AttendanceRecords.Add(efRecord);
            await Task.CompletedTask;
        }

        public async Task UpdateAsync(Domain.Entities.AttendanceRecord domainRecord)
        {
            // Fetch the existing EF record.
            var efRecord = await _context.AttendanceRecords
                .Include(r => r.AttendanceSessions)
                .FirstOrDefaultAsync(r => r.Id == domainRecord.Id);

            if (efRecord == null)
                throw new Exception("Record not found");

            // Update basic properties.
            efRecord.TotalWorkDuration = domainRecord.TotalWorkDuration;
            efRecord.TotalBreakDuration = domainRecord.TotalBreakDuration;
            efRecord.Date = DateOnly.FromDateTime(domainRecord.Date);
            efRecord.UserId = domainRecord.UserId ?? 0;

            // For simplicity, clear and replace sessions.
            efRecord.AttendanceSessions.Clear();
            foreach (var session in domainRecord.Sessions)
            {
                efRecord.AttendanceSessions.Add(new Models.AttendanceSession
                {
                    Id = session.Id,
                    EntryTime = session.EntryTime,
                    ExitTime = session.ExitTime,
                    AttendanceRecordId = session.AttendanceRecordId
                });
            }

            _context.AttendanceRecords.Update(efRecord);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
