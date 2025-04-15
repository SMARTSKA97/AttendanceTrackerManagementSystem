using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class AttendanceSession
{
    public int Id { get; set; }

    public int AttendanceRecordId { get; set; }

    public DateTime? EntryTime { get; set; }

    public DateTime? ExitTime { get; set; }

    public virtual AttendanceRecord AttendanceRecord { get; set; } = null!;
}
