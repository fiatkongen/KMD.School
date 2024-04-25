using System.Linq;
using FluentAssertions;
using KMD.School.Infrastructure;
using KMD.School.Model;
using Xunit;


namespace KMD.School.Test.Unit;

public class DanishRandomNameGeneratorTest
{
    [Fact]
    public async void GenerateRandomUniqueNames_Generates_Expected_Number_Of_Names()
    {
        // Arrange
        var generator = new DanishRandomNameGenerator();
        int numberOfNames = 10;

        // Act
        var names = await generator.GenerateRandomUniqueNames(numberOfNames);

        // Assert
        names.Should().HaveCount(numberOfNames);
    }

    [Fact]
    public async void GenerateRandomUniqueNames_Generates_Unique_Names()
    {
        // Arrange
        var generator = new DanishRandomNameGenerator();
        int numberOfNames = 10;

        // Act
        var names = await generator.GenerateRandomUniqueNames(numberOfNames);

        // Assert
        names.Distinct().Should().HaveSameCount(names);
    }

    [Fact]
    public async void GenerateRandomUniqueNames_Generates_Names_With_First_And_Last_Name()
    {
        // Arrange
        var generator = new DanishRandomNameGenerator();
        int numberOfNames = 10;

        // Act
        var names = await generator.GenerateRandomUniqueNames(numberOfNames);

        // Assert
        foreach (var name in names)
        {
            name.Split(' ').Should().HaveCount(2);
        }
    }
}