using System.ComponentModel.DataAnnotations;

namespace StudentManagment.DTOs.CafeAccess
{
    public class CreateCafeAccessDto
    {
        [Required(ErrorMessage = "Student ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Student ID must be a positive integer.")]
        public required int StudentId { get; set; }

        [Required(ErrorMessage = "A scannable ID code is required.")]
        [StringLength(50, ErrorMessage = "Scannable ID code cannot exceed 50 characters.")]
        public required string ScannableIdCode { get; set; }
    }
}
