using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HCAMiniEHR.Models;

[Table("Appointment", Schema = "Healthcare")]
public class Appointment
{
    [Key]
    public int AppointmentId { get; set; }

    [Required]
    public int PatientId { get; set; }

    [Required]
    //[FutureDate]
    public DateTime AppointmentDate { get; set; }

    [Required]
    public TimeSpan AppointmentTime { get; set; }

    [MaxLength(100)]
    public string DoctorName { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Reason { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "Scheduled";

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    // Navigation properties
    [ForeignKey("PatientId")]
    public Patient Patient { get; set; } = null!;

    public ICollection<LabOrder> LabOrders { get; set; } = new List<LabOrder>();
    public int? DoctorId { get; set; }

    [ForeignKey("DoctorId")]
    public Doctor? Doctor { get; set; }

}
