using System.Net.Http.Json;
using AutoFixture;
using FluentAssertions;
using KMD.School.Dto;
using KMD.School.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using KMD.School.Model;

namespace KMD.School.Test.Integration;

public class StudentIntegrationTest
{
    private readonly WebApplicationFactory<Program> _factory;

    public StudentIntegrationTest()
    {
        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<AppDbContext>));
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("test");
                });
            });
        });
    }

    [Fact]
    public async void given_student_exists_when_getting_student_then_return_student()
    {
        //Arrange
        int createStudentsCount = 3;
        var testStudents = new Fixture().Build<Student>().CreateMany(createStudentsCount).ToList();

        await SeedDb(testStudents);

        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(HttpHelper.Urls.Student + "/" + testStudents.First().Id);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var firstStudent = await response.Content.ReadFromJsonAsync<StudentDto>();
        firstStudent.Should().BeEquivalentTo(testStudents.First(), 
            options => options.ExcludingMissingMembers());  //ExcludingMissingMembers since the DTO lacks the property IsMarkedAsDeleted 
    }

    [Fact]
    public async void given_students_exists_when_getting_all_student_then_return_all_students()
    {
        //Arrange
        int createStudentsCount = 3;
        var testStudents = new Fixture().Build<Student>().CreateMany(createStudentsCount).ToList();

        await SeedDb(testStudents);

        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(HttpHelper.Urls.AllStudents);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var allStudents = await response.Content.ReadFromJsonAsync<List<StudentDto>>();
        allStudents.Count.Should().Be(createStudentsCount);
        allStudents.Should().BeEquivalentTo(testStudents, options => options.ExcludingMissingMembers());
    }

    [Fact]
    public async void given_student_exists_when_deleting_student_then_return_ok()
    {
        //Arrange
        int createStudentsCount = 3;
        var testStudents = new Fixture().Build<Student>().CreateMany(createStudentsCount).ToList();

        await SeedDb(testStudents);

        var client = _factory.CreateClient();

        // Act
        var response = await client.DeleteAsync(HttpHelper.Urls.Student + "/" + testStudents.First().Id);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async void given_student_exists_when_deleting_student_and_requesting_all_students_then_return_remaining_students()
    {
        //Arrange
        int createStudentsCount = 3;
        var testStudents = new Fixture().Build<Student>().CreateMany(createStudentsCount).ToList();

        await SeedDb(testStudents);

        var client = _factory.CreateClient();

        // Act
        await client.DeleteAsync(HttpHelper.Urls.Student + "/" + testStudents.First().Id);
        var response = await client.GetAsync(HttpHelper.Urls.AllStudents);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var allStudents = await response.Content.ReadFromJsonAsync<List<StudentDto>>();
        allStudents.Count.Should().Be(createStudentsCount - 1);
    }

    [Fact]
    public async void given_3_students_exists_when_deleting_all_students_then_return_ok()
    {
        //Arrange
        int createStudentsCount = 3;
        var testStudents = new Fixture().Build<Student>().CreateMany(createStudentsCount).ToList();

        await SeedDb(testStudents);

        var client = _factory.CreateClient();

        // Act
        await client.DeleteAsync(HttpHelper.Urls.AllStudents);
        var response = await client.GetAsync(HttpHelper.Urls.AllStudents);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var allStudents = await response.Content.ReadFromJsonAsync<List<StudentDto>>();
        allStudents.Count.Should().Be(0);
    }

    [Fact]
    public async void when_creating_a_student_then_create_student_and_return_ok()
    {
        //Arrange
        await SeedDb(new List<Student>());
        var client = _factory.CreateClient();

        // Act
        var createStudentDto = new Fixture().Build<CreateStudentDto>().Create();
        var responseCreateStudent = await client.PostAsJsonAsync(HttpHelper.Urls.Student, createStudentDto);

        // Assert
        responseCreateStudent.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var responseAllStudents = await client.GetAsync(HttpHelper.Urls.AllStudents);
        var allStudents = await responseAllStudents.Content.ReadFromJsonAsync<List<StudentDto>>();
        allStudents.Count.Should().Be(1);
        allStudents.First().Should().BeEquivalentTo(createStudentDto);
        allStudents.First().CreatedDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromHours(1));
    }

    [Fact]
    public async void given_student_exists_when_modifying_the_student_then_update_student_and_return_ok()
    {
        //Arrange
        int createStudentsCount = 1;
        var testStudents = new Fixture().Build<Student>().CreateMany(createStudentsCount).ToList();

        await SeedDb(testStudents);
        var client = _factory.CreateClient();

        // Act
        var modifyStudentDto = new Fixture().Build<UpdateStudentDto>().Create();
        modifyStudentDto.Id = testStudents.First().Id;
        var responseCreateStudent = await client.PutAsJsonAsync(HttpHelper.Urls.Student, modifyStudentDto);

        // Assert
        responseCreateStudent.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var responseAllStudents = await client.GetAsync(HttpHelper.Urls.AllStudents);
        var allStudents = await responseAllStudents.Content.ReadFromJsonAsync<List<StudentDto>>();
        allStudents.First().Should().BeEquivalentTo(modifyStudentDto, options => options.ExcludingMissingMembers());
    }

    [Fact]
    public async void when_creating_a_random_number_of_students_then_create_random_students_and_return_ok()
    {
        //Arrange
        int randomStudentsCount = 10;
        await SeedDb(new List<Student>());
        var client = _factory.CreateClient();

        // Act
        var responseCreateStudent = await client.PostAsync(HttpHelper.Urls.RandomStudents + "?randomStudentsCount=" + randomStudentsCount, null);

        // Assert
        responseCreateStudent.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        
        var responseAllStudents = await client.GetAsync(HttpHelper.Urls.AllStudents);
        var allStudents = await responseAllStudents.Content.ReadFromJsonAsync<List<StudentDto>>();
        allStudents.Count.Should().Be(randomStudentsCount);
    }

    private async Task SeedDb(List<Student> testStudents)
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            dbContext.Students.AddRange(testStudents);

            await dbContext.SaveChangesAsync();
        }
    }
}