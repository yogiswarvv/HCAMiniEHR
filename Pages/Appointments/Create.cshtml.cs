using HCAMiniEHR.Models;
using HCAMiniEHR.Services;
using HCAMiniEHR.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HCAMiniEHR.Pages.Appointments;

public class CreateModel : PageModel
{
    private readonly AppointmentService _appointmentService;
    private readonly PatientService _patientService;
    private readonly DoctorService _doctorService;

    public CreateModel(
        AppointmentService appointmentService,
        PatientService patientService,
        DoctorService doctorService)
    {
        _appointmentService = appointmentService;
        _patientService = patientService;
        _doctorService = doctorService;
    }

    [BindProperty]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a patient")]
    public int PatientId { get; set; }

    [BindProperty]
    [Required]
    [FutureDate(ErrorMessage = "Appointment date cannot be in the past.")]
    public DateTime AppointmentDate { get; set; } = DateTime.Now;

    [BindProperty]
    [Required]
    public TimeSpan AppointmentTime { get; set; } = new TimeSpan(9, 0, 0);

    [BindProperty]
    public string? Reason { get; set; }

    public SelectList Patients { get; set; } = null!;
    public SelectList Doctors { get; set; } = null!;

    [BindProperty]
    [Required(ErrorMessage = "Doctor selection is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a doctor")]
    public int DoctorId { get; set; }


    public async Task OnGetAsync(int? patientId)
    {
        var patients = await _patientService.GetAllPatientsAsync();
        Patients = new SelectList(patients, "PatientId", "FullName");

        var doctors = await _doctorService.GetAvailableDoctorsAsync();
        Doctors = new SelectList(doctors, "DoctorId", "FullName");

        if (patientId.HasValue)
        {
            PatientId = patientId.Value;
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (AppointmentDate.Date == DateTime.Now.Date && AppointmentTime < DateTime.Now.TimeOfDay)
        {
            ModelState.AddModelError("AppointmentTime", "Appointment time cannot be in the past.");
        }

        if (!ModelState.IsValid)
        {
            await PopulateSelectListsAsync();
            return Page();
        }

        try
        {
            var newId = await _appointmentService.CreateAppointmentUsingStoredProcAsync(
                                             PatientId, AppointmentDate, AppointmentTime, DoctorId, Reason);

            TempData["SuccessMessage"] =
                $"Appointment created successfully! (ID: {newId})";

            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            await PopulateSelectListsAsync();
            return Page();
        }
    }

    private async Task PopulateSelectListsAsync()
    {
        var patients = await _patientService.GetAllPatientsAsync();
        Patients = new SelectList(patients, "PatientId", "FullName", PatientId);

        var doctors = await _doctorService.GetAvailableDoctorsAsync();
        Doctors = new SelectList(doctors, "DoctorId", "FullName", DoctorId);
    }
}
