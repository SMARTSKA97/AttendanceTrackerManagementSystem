using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class Record
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly? EntryTime { get; set; }

    public TimeOnly? ExitTime { get; set; }

    public TimeSpan? WorkDuration { get; set; }

    public string? Notes { get; set; }
}
