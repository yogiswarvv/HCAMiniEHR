using HCAMiniEHR.Data;
using HCAMiniEHR.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HCAMiniEHR.Pages.Reports;

public class IndexModel : PageModel
{
    private readonly EhrDbContext _context;

    public IndexModel(EhrDbContext context)
    {
        _context = context;
    }

    // Report 1: Pending Lab Orders
    public IEnumerable<PendingLabOrderReport> PendingLabOrders { get; set; } = new List<PendingLabOrderReport>();

    // Report 2: Patients Without Follow-Up
    public IEnumerable<PatientWithoutFollowUpReport> PatientsWithoutFollowUp { get; set; } = new List<PatientWithoutFollowUpReport>();

    // Report 3: Doctor Productivity
    public IEnumerable<DoctorProductivityReport> DoctorProductivity { get; set; } = new List<DoctorProductivityReport>();

    public async Task OnGetAsync()
    {
        // Report 1: Pending Lab Orders using LINQ (Where, Include, OrderBy)
        PendingLabOrders = await _context.LabOrders
            .Include(lo => lo.Appointment)
                .ThenInclude(a => a.Patient)
            .Where(lo => lo.Status == "Pending")
            .OrderBy(lo => lo.OrderDate)
            .Select(lo => new PendingLabOrderReport
            {
                LabOrderId = lo.LabOrderId,
                TestName = lo.TestName,
                PatientName = lo.Appointment.Patient.FirstName + " " + lo.Appointment.Patient.LastName,
                OrderDate = lo.OrderDate,
                AppointmentDate = lo.Appointment.AppointmentDate,
                DoctorName = lo.Appointment.Doctor.FirstName + " " + lo.Appointment.Doctor.LastName
            })
            .ToListAsync();

        // Report 2: Patients Without Follow-Up using LINQ (Where with Any, Select, OrderBy)
        var today = DateTime.Now;
        PatientsWithoutFollowUp = await _context.Patients
            .Include(p => p.Appointments)
            .Where(p => !p.Appointments.Any(a => a.AppointmentDate > today))
            .Select(p => new PatientWithoutFollowUpReport
            {
                PatientId = p.PatientId,
                PatientName = p.FirstName + " " + p.LastName,
                Phone = p.Phone ?? "N/A",
                Email = p.Email ?? "N/A",
                LastAppointmentDate = p.Appointments
                    .OrderByDescending(a => a.AppointmentDate)
                    .Select(a => (DateTime?)a.AppointmentDate)
                    .FirstOrDefault(),
                TotalAppointments = p.Appointments.Count
            })
            .OrderBy(p => p.PatientName)
            .ToListAsync();

        // Report 3: Doctor Productivity using LINQ (GroupBy, Select, OrderByDescending)
        DoctorProductivity = await _context.Doctors
            .Include(d => d.Appointments)
                .ThenInclude(a => a.LabOrders)
            .Select(d => new DoctorProductivityReport
            {
                DoctorId = d.DoctorId,
                DoctorName = d.FirstName + " " + d.LastName,
                Specialization= d.Specialization,
                TotalAppointments = d.Appointments.Count,
                TotalLabOrders = d.Appointments.SelectMany(a => a.LabOrders).Count()
            })
            .OrderByDescending(r => r.TotalAppointments)
            .ThenBy(r => r.DoctorName)
            .ToListAsync();
    }
}

// DTOs for Reports
public class PendingLabOrderReport
{
    public int LabOrderId { get; set; }
    public string TestName { get; set; } = string.Empty;
    public string PatientName { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string DoctorName { get; set; } = string.Empty;
}

public class PatientWithoutFollowUpReport
{
    public int PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime? LastAppointmentDate { get; set; }
    public int TotalAppointments { get; set; }
}

public class DoctorProductivityReport
{
    public int DoctorId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public string? Specialization{ get; set; }
    public int TotalAppointments { get; set; }
    public int TotalLabOrders { get; set; }
}

