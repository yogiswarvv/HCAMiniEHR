namespace HCAMiniEHR.DTOs;

public class LabOrderListDto
{
    public int LabOrderId { get; set; }
    public string TestName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;

    public DateTime OrderDate { get; set; }
    public DateTime AppointmentDate { get; set; }

    public string PatientName { get; set; } = string.Empty;
    public string? Results { get; set; }
}
