using HCAMiniEHR.Models;
using HCAMiniEHR.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HCAMiniEHR.Pages.LabOrders;

public class CreateModel : PageModel
{
    private readonly LabOrderService _labOrderService;
    private readonly AppointmentService _appointmentService;

    public CreateModel(LabOrderService labOrderService, AppointmentService appointmentService)
    {
        _labOrderService = labOrderService;
        _appointmentService = appointmentService;
    }

    [BindProperty]
    [Required(ErrorMessage = "The Appointment field is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Please select an appointment.")]
    public int AppointmentId { get; set; }

    [BindProperty]
    [Required]
    [MaxLength(200)]
    public string TestName { get; set; } = string.Empty;

    public SelectList Appointments { get; set; } = null!;

    public async Task OnGetAsync(int? appointmentId)
    {
        await PopulateAppointmentsAsync();
        if (appointmentId.HasValue)
        {
            AppointmentId = appointmentId.Value;
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await PopulateAppointmentsAsync();
            return Page();
        }

        var labOrder = new LabOrder
        {
            AppointmentId = this.AppointmentId,
            TestName = this.TestName
        };

        try
        {
            await _labOrderService.CreateLabOrderAsync(labOrder);
            TempData["SuccessMessage"] = $"Lab order for {labOrder.TestName} created successfully!";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            await PopulateAppointmentsAsync();
            return Page();
        }
    }

    private async Task PopulateAppointmentsAsync()
    {
        var appointments = await _appointmentService.GetAllAppointmentsAsync();
        Appointments = new SelectList(
            appointments.Select(a => new
            {
                a.AppointmentId,
                DisplayText = $"Apt #{a.AppointmentId} - {a.Patient.FullName} - {a.AppointmentDate:MM/dd/yyyy}"
            }),
            "AppointmentId",
            "DisplayText"
        );
    }
}
