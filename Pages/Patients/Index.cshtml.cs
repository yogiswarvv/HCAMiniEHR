using HCAMiniEHR.DTOs;
using HCAMiniEHR.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HCAMiniEHR.Pages.Patients;

public class IndexModel : PageModel
{
    private readonly PatientService _patientService;

    public IndexModel(PatientService patientService)
    {
        _patientService = patientService;
    }

    public List<PatientListDto> Patients { get; set; } = new();

    public async Task OnGetAsync()
    {
        Patients = await _patientService.GetPatientListAsync();
    }
}
