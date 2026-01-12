using HCAMiniEHR.DTOs;
using HCAMiniEHR.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HCAMiniEHR.Pages.Appointments;

public class IndexModel : PageModel
{
    private readonly AppointmentService _appointmentService;

    public IndexModel(AppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    public List<AppointmentListDto> Appointments { get; set; } = new();

    public async Task OnGetAsync()
    {
        Appointments = await _appointmentService.GetAppointmentListAsync();
    }
}
