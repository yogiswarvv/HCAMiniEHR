using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HCAMiniEHR.Models;

[Table("Patient", Schema = "Healthcare")]
public class Patient
{
    [Key]
    public int PatientId { get; set; }

    [Required]
    [MaxLength(100)]
    [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "First name can only contain letters.")]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Last name can only contain letters.")]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    [Required]
    [MaxLength(10)]
    public string? Gender { get; set; }

    [Required(ErrorMessage = "Phone number is required")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits")]
    public string Phone { get; set; } = string.Empty;


    // ✅ EMAIL VALIDATION
    [Required]
    [EmailAddress(ErrorMessage = "Enter a valid email address")]
    [MaxLength(100)]
    public string? Email { get; set; }

    [MaxLength(200)]
    public string? Address { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    // Navigation property
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";
}
