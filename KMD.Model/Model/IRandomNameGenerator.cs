namespace KMD.School.Model;

public interface IRandomNameGenerator
{
    Task<List<string>> GenerateRandomUniqueNames(int numberOfNames);
}