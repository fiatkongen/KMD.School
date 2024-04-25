using KMD.School.Model;
using Microsoft.EntityFrameworkCore;

namespace KMD.School.Infrastructure;

public class StudentRepository : IStudentRepository
{
    private readonly AppDbContext _dbContext;

    public StudentRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddStudentAsync(Student student)
    {
        await _dbContext.Students.AddAsync(student);
    }

    public void RemoveStudentAsync(Student student)
    {
        _dbContext.Students.Remove(student);
    }

    public void UpdateStudentAsync(Student student)
    {
        _dbContext.Students.Update(student);
    }

    public Task<Student?> GetStudentAsync(Guid id)
    {
        return _dbContext.Students.SingleOrDefaultAsync(x => x.Id == id);
    }

    public Task<List<Student>> GetAllStudentsAsync()
    {
        return _dbContext.Students.ToListAsync();
    }
}