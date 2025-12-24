using System.ComponentModel.DataAnnotations;

namespace StudentManagment.DTOs.CafeAccess
{
    public class CafeScanRequestDto
    {
        [Required(ErrorMessage = "Scannable ID code is required for access.")]
        [StringLength(50)]
        public required string ScannableIdCode { get; set; }
    }
}
