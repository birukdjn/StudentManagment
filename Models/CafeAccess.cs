using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StudentManagment.Models { 

    [Index(nameof(ScannableIdCode), IsUnique = true)] 

    public class CafeAccess: IValidatableObject
    {
        [Key]
        public int CafeAccessId { get; set; }

        public required int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public Student Student { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public required string ScannableIdCode { get; set; }

        public bool HasAccessedBreakfast { get; set; } = false;
        public bool HasAccessedLunch { get; set; } = false;
        public bool HasAccessedDinner { get; set; } = false;

        public DateTime LastResetDate { get; set; } = DateTime.UtcNow.Date;

        public int TotalDailyAccesses { get; set; } = 0;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {  
            if (Student.ReceivesCashAllowance)
            {
                yield return new ValidationResult("Students receiving cash allowance should not have CafeAccess records.", 
                    [nameof(Student.ReceivesCashAllowance)]);
            }
            if (!string.IsNullOrWhiteSpace(Student.AccountNumber))
            {
                yield return new ValidationResult(
                    "Students with CafeAccess should not have an AccountNumber.",
                    [nameof(Student.AccountNumber)]
                );
            }

        }


    }
}