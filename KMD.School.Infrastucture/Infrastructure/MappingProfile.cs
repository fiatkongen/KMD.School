using AutoMapper;
using KMD.School.Dto;
using KMD.School.Model;

namespace KMD.School.Infrastucture.Infrastructure;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Student, StudentDto>();
        CreateMap<CreateStudentDto, Student>();
        CreateMap<UpdateStudentDto, Student>();
    }
}