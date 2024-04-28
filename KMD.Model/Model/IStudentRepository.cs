namespace KMD.School.Model;

public interface IStudentRepository
{
    Task AddStudentAsync(Student student);
    void RemoveStudentAsync(Student student);
    void UpdateStudentAsync(Student student);
    Task<Student?> GetStudentAsync(Guid id);
    Task<List<Student>> GetAllStudentsAsync();
}