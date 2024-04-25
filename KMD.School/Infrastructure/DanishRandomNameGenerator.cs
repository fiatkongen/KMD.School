using KMD.School.Model;

namespace KMD.School.Infrastructure;

public class DanishRandomNameGenerator : IRandomNameGenerator
{
    private List<string> _danishFirstNames = new()
    {
        "Emma", "Freja", "Alma", "Sofia", "Clara", "Olivia", "Ella", "Anna", "Karla", "Laura",
        "Ida", "Maja", "Josefine", "Lærke", "Mathilde", "Sofie", "Alberte", "Isabella", "Agnes", "Frida",
        "Caroline", "Emilie", "Victoria", "Mille", "Liva", "Asta", "Ellie", "Nora", "Emily", "Lily",
        "Sara", "Andrea", "Marie", "Ellen", "Cecilie", "Lea", "Filippa", "Hannah", "Vigga", "Rosa",
        "Vilma", "Selma", "Esther", "Johanne", "Merle", "Aya", "Thea", "Liv", "Luna", "Signe"
    };


    private List<string> _danishLastNames = new()
    {
        "Jensen", "Nielsen", "Hansen", "Pedersen", "Andersen", "Christensen", "Larsen", "Sørensen", "Rasmussen", "Jørgensen",
        "Petersen", "Madsen", "Kristensen", "Olsen", "Thomsen", "Christiansen", "Poulsen", "Johansen", "Møller", "Mortensen",
        "Knudsen", "Jakobsen", "Mikkelsen", "Olesen", "Frederiksen", "Svendsen", "Laursen", "Henriksen", "Lund", "Schmidt",
        "Eriksen", "Holm", "Kristiansen", "Clausen", "Simonsen", "Ravn", "Dalgaard", "Johnsen", "Kjær", "Steffensen",
        "Andresen", "Mogensen", "Iversen", "Dam", "Jeppesen", "Fischer", "Bertelsen", "Lassen", "Friis", "Hauge"
    };

    //In a future version, use a call to LLM-api to get the names
    public async Task<List<string>> GenerateRandomUniqueNames(int numberOfNames)
    {
        var random = new Random();
        var names = new HashSet<string>();

        while (names.Count < numberOfNames)
        {
            var firstName = _danishFirstNames[random.Next(_danishFirstNames.Count)];
            var lastName = _danishLastNames[random.Next(_danishLastNames.Count)];
            var fullName = $"{firstName} {lastName}";

            names.Add(fullName);
        }

        return new List<string>(names);
    }
}