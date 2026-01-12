using System.ComponentModel.DataAnnotations;

namespace HCAMiniEHR.Validation
{
    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                // Let the [Required] attribute handle null values
                return true;
            }

            if (value is DateTime dateTime)
            {
                return dateTime.Date >= DateTime.Now.Date;
            }
            return false;
        }
    }
}
