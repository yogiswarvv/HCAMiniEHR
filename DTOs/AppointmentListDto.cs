namespace HCAMiniEHR.DTOs;

public class AppointmentListDto
{
    public int AppointmentId { get; set; }

    public string PatientName { get; set; } = string.Empty;

    public DateTime AppointmentDate { get; set; }
    public TimeSpan AppointmentTime { get; set; }

    public string DoctorName { get; set; } = string.Empty;
    public string? Reason { get; set; }

    public string Status { get; set; } = string.Empty;

    public int LabOrderCount { get; set; }
}
