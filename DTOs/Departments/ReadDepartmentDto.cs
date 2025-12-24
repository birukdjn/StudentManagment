
namespace StudentManagment.DTOs.Departments
{
    public class ReadDepartmentDto
    {
        public int DepartmentId { get; set; }
        public required string Name { get; set; }
        public bool IsActive { get; set; }
    }
}