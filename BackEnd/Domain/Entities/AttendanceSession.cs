using System;

namespace Domain.Entities
{
    public class AttendanceSession
    {
        public int Id { get; set; }
        public DateTime? EntryTime { get; set; }
        public DateTime? ExitTime { get; set; }
        public int AttendanceRecordId { get; set; }
    }
}
