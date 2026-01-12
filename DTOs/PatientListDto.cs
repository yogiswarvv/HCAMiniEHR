namespace HCAMiniEHR.DTOs;

public class PatientListDto
{
    public int PatientId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public string? Gender { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }

    public int AppointmentCount { get; set; }
}
