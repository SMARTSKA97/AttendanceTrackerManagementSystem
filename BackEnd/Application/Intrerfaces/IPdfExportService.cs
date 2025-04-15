using Domain.Entities;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IPdfExportService
    {
        byte[] ExportAttendanceToPdf(List<AttendanceRecord> records, int year, int month);
    }
}
