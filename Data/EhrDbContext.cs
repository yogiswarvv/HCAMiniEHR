using Microsoft.EntityFrameworkCore;
using HCAMiniEHR.Models;

namespace HCAMiniEHR.Data;

public class EhrDbContext : DbContext
{
    public EhrDbContext(DbContextOptions<EhrDbContext> options) : base(options)
    {
    }

    public DbSet<Patient> Patients { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<LabOrder> LabOrders { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<Doctor> Doctors { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.HasDefaultSchema("Healthcare");

        // Configure Patient entity
        modelBuilder.Entity<Patient>(entity =>
        {
            entity.ToTable("Patient", "Healthcare");
            entity.HasKey(p => p.PatientId);
            entity.Property(p => p.CreatedDate).HasDefaultValueSql("GETDATE()");
            
            // Configure one-to-many relationship with Appointments
            entity.HasMany(p => p.Appointments)
                  .WithOne(a => a.Patient)
                  .HasForeignKey(a => a.PatientId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Appointment entity
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.ToTable("Appointment", "Healthcare");
            entity.HasKey(a => a.AppointmentId);
            entity.Property(a => a.Status).HasDefaultValue("Scheduled");
            entity.Property(a => a.CreatedDate).HasDefaultValueSql("GETDATE()");
            
            // Configure one-to-many relationship with LabOrders
            entity.HasMany(a => a.LabOrders)
                  .WithOne(l => l.Appointment)
                  .HasForeignKey(l => l.AppointmentId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Doctor entity
        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.ToTable("Doctor", "Healthcare");
            entity.HasKey(d => d.DoctorId);
            entity.Property(d => d.CreatedDate)
                  .HasDefaultValueSql("GETDATE()");
        });

        // Configure Appointment ↔ Doctor relationship
        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Doctor)
            .WithMany(d => d.Appointments)
            .HasForeignKey(a => a.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure LabOrder entity
        modelBuilder.Entity<LabOrder>(entity =>
        {
            entity.ToTable("LabOrder", "Healthcare");
            entity.HasKey(l => l.LabOrderId);
            entity.Property(l => l.Status).HasDefaultValue("Pending");
            entity.Property(l => l.OrderDate).HasDefaultValueSql("GETDATE()");
        });

        // Configure AuditLog entity
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.ToTable("AuditLog", "Healthcare");
            entity.HasKey(a => a.AuditLogId);
            entity.Property(a => a.ChangedDate).HasDefaultValueSql("GETDATE()");
            entity.Property(a => a.ChangedBy).HasDefaultValue("SYSTEM");
        });
    }
}
