using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Models;

public partial class AttendanceDbContext : DbContext
{
    public AttendanceDbContext(DbContextOptions<AttendanceDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AttendanceRecord> AttendanceRecords { get; set; }

    public virtual DbSet<AttendanceSession> AttendanceSessions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AttendanceRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("attendance_record_pkey");

            entity.ToTable("attendance_record", "attendance");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.TotalBreakDuration).HasColumnName("total_break_duration");
            entity.Property(e => e.TotalWorkDuration).HasColumnName("total_work_duration");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<AttendanceSession>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("attendance_session_pkey");

            entity.ToTable("attendance_session", "attendance");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AttendanceRecordId).HasColumnName("attendance_record_id");
            entity.Property(e => e.EntryTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("entry_time");
            entity.Property(e => e.ExitTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("exit_time");

            entity.HasOne(d => d.AttendanceRecord).WithMany(p => p.AttendanceSessions)
                .HasForeignKey(d => d.AttendanceRecordId)
                .HasConstraintName("fk_attendance_record");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
