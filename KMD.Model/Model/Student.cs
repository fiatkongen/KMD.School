namespace KMD.School.Model;


[Serializable]
public class Student 
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public DateTime DateOfBirth { get; set; }
    public bool IsMarkedAsDeleted { get; set; }

    private Student() { } //for Automapper

    public Student(Guid id, string firstName, string lastName, string address, DateTime dateOfBirth) 
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Address = address;
        DateOfBirth = dateOfBirth;
    }
}