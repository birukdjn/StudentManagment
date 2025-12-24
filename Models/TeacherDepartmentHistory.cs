using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagment.Models
{
    public class TeacherDepartmentHistory
    {
        [Key]
        public int HistoryId { get; set; }

        public required int TeacherId { get; set; }
        [ForeignKey(nameof(TeacherId))]
        public Teacher Teacher { get; set; } = null!;

        public required int DepartmentId { get; set; }
        [ForeignKey(nameof(DepartmentId))]
        public Department Department { get; set; } = null!;

        public required DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
    }
}