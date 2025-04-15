using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class AttendanceRecord
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan? TotalWorkDuration { get; set; }
        public TimeSpan? TotalBreakDuration { get; set; }
        public int? UserId { get; set; }

        // Domain property named "Sessions"
        public List<AttendanceSession> Sessions { get; set; } = new List<AttendanceSession>();
    }
}
