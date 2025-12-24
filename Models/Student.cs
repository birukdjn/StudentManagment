using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagment.Models
{
    [Index(nameof(AccountNumber), IsUnique = true)]
    public class Student : IValidatableObject
    {
        [Key]
        public int StudentId { get; set; }

        public DateOnly EnrollmentDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
        [Range(0.0, 4.0)]
        public double GPA { get; set; } = 0.0;

        public bool IsEnrolled { get; set; } = true;
        public bool IsAlumni => !IsEnrolled;
        public bool ReceivesCashAllowance { get; set; } = false;
        public string? AccountNumber { get; set; }
        public required int DepartmentId { get; set; }
        [ForeignKey(nameof(DepartmentId))]
        public Department? Department { get; set; }
        public required int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

        public CafeAccess? CafeAccess { get; set; }

        public ICollection<Enrollment>? Enrollments { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ReceivesCashAllowance && string.IsNullOrWhiteSpace(AccountNumber))
            {
                yield return new ValidationResult(
                    "AccountNumber is required when ReceivesCashAllowance is true.",
                  [nameof(AccountNumber)]
                  );
            }
            if (!ReceivesCashAllowance && !string.IsNullOrWhiteSpace(AccountNumber))
            {
                yield return new ValidationResult
                (
                    "AccountNumber should be null or empty when ReceivesCashAllowance is false.",
                  [nameof(AccountNumber)]
                );
            }

            if(ReceivesCashAllowance && !string.IsNullOrWhiteSpace(CafeAccess?.ScannableIdCode))
            {
                yield return new ValidationResult
                (
                    "Student with CafeAccess cannot receive cash allowance.",
                  [nameof(CafeAccess)]
                );
            }
            if (!ReceivesCashAllowance && CafeAccess == null)
            {
                yield return new ValidationResult
                (
                    "Student without CafeAccess must receive cash allowance.",
                  [nameof(CafeAccess)]
                );
            }
        }
    }
}
