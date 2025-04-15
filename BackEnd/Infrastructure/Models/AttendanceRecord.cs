using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class AttendanceRecord
{
    public int Id { get; set; }

    public DateOnly Date { get; set; }

    public TimeSpan? TotalWorkDuration { get; set; }

    public TimeSpan? TotalBreakDuration { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<AttendanceSession> AttendanceSessions { get; set; } = new List<AttendanceSession>();
}
