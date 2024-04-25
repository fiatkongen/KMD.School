using System.ComponentModel.DataAnnotations;

namespace KMD.School.Dto;

public class CreateStudentDto
{
    [Required] public string FirstName { get; set; }
    [Required] public string LastName { get; set; }
    [Required] public string Address { get; set; }
    public DateTime DateOfBirth { get; set; }
}

public class StudentDto : CreateStudentDto
{
    public Guid Id { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class UpdateStudentDto : CreateStudentDto
{
    public new Guid Id { get; set; }
    public bool IsMarkedAsDeleted { get; set; }
}