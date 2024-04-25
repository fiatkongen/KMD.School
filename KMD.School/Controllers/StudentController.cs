using AutoMapper;
using KMD.School.Dto;
using KMD.School.Infrastructure;
using KMD.School.Model;
using Microsoft.AspNetCore.Mvc;
using System;

namespace KMD.School.Controllers;

[ApiController]
[Route("api/student")]
public class StudentController(IStudentRepository studentRepository, AppDbContext appDbContext, IMapper mapper, IRandomNameGenerator _randomNameGenerator) : ControllerBase
{
    private readonly AppDbContext _appDbContext = appDbContext;
    private readonly IStudentRepository _studentRepository = studentRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IRandomNameGenerator _randomNameGenerator = _randomNameGenerator;

    [HttpGet("{id}")]
    public async Task<ObjectResult> GetStudentAsync(Guid id)
    {
        var student = await _studentRepository.GetStudentAsync(id);
        if (student is null)
        {
            return new ObjectResult(null);
        }
        var dto = _mapper.Map<StudentDto>(student);
        return new ObjectResult(dto);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllStudentsAsync()
    {
        var students = await _studentRepository.GetAllStudentsAsync();
        var dtos = _mapper.Map<List<StudentDto>>(students);
        return Ok(dtos);
    }

    [HttpPost("")]
    public async Task<IActionResult> CreateStudentAsync(CreateStudentDto studentDto)
    {
        var student = _mapper.Map<Student>(studentDto);
        await _studentRepository.AddStudentAsync(student);
        await _appDbContext.SaveChangesAsync();

        return Ok();
    }

    [HttpPut("")]
    public async Task<IActionResult> UpdateStudentAsync(UpdateStudentDto studentDto)
    {
        var student = await _studentRepository.GetStudentAsync(studentDto.Id);
        _mapper.Map(studentDto, student);
        _studentRepository.UpdateStudentAsync(student);
        int result = await _appDbContext.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var student = await _studentRepository.GetStudentAsync(id);
        if (student is null)
        {
            return new ObjectResult(null);
        }
        _studentRepository.RemoveStudentAsync(student);
        await _appDbContext.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("random")]
    public async Task<IActionResult> CreateRandomStudentsAsync(int randomStudentsCount = 10)
    {
        var names = await _randomNameGenerator.GenerateRandomUniqueNames(randomStudentsCount);
        var students = names.Select(x => new Student(
            Guid.NewGuid(), 
            x.Split(" ")[0], 
            x.Split(" ")[1],
            "Skolevej",
            DateTime.Today.AddYears(0 - new Random().Next(6, 16)))).ToList();

        foreach (var student in students)
        {
            await _studentRepository.AddStudentAsync(student);
        }
        await _appDbContext.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("all")]
    public async Task<IActionResult> DeleteAllAsync()
    {
        var students = await _studentRepository.GetAllStudentsAsync();
        foreach (var student in students)
        {
            _studentRepository.RemoveStudentAsync(student);
        }
        await _appDbContext.SaveChangesAsync();
        return Ok();
    }
}
