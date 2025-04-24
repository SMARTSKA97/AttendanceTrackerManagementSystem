using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Domain.Entities;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpGet]
        public async Task<ActionResult<List<AttendanceRecord>>> GetAll()
        {
            var records = await _attendanceService.GetAllAsync();
            return Ok(records);
        }

        [HttpGet("today")]
        public async Task<ActionResult<AttendanceRecord?>> GetToday()
        {
            var record = await _attendanceService.GetTodayAsync();
            return Ok(record);
        }

        // This endpoint toggles check in/out.
        [HttpPost("toggle")]
        public async Task<ActionResult<AttendanceRecord>> ToggleCheck()
        {
            try
            {
                var record = await _attendanceService.ToggleCheckAsync();
                return Ok(record);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PDF export endpoint updated to include sessions, total work and break durations.
        [HttpGet("export")]
        public async Task<IActionResult> ExportAttendance([FromQuery] int year, [FromQuery] int month)
        {
            try
            {
                var pdfBytes = await _attendanceService.ExportMonthlyPdfAsync(year, month);
                return File(pdfBytes, "application/pdf", $"Attendance_{year}_{month}.pdf");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("status")]
        public async Task<IActionResult> GetStatus()
        {
            try
            {
                var isCheckedOut = await _attendanceService.IsCheckedOutAsync();
                return Ok(new { isCheckedOut });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
