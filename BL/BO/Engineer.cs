
namespace BO;

/// <summary>
/// Engineer Entity represents a engineer with all its properties.
/// </summary>
/// <param name="Id">the engineer's id</param>
/// <param name="Name">the engineer's name</param>
/// <param name="Email">the engineer's email address</param>
/// <param name="Level"> the engineer's experience level</param>
/// <param name="SaleryPerHour">the amount the engineer gets per hour</param>
/// <param name="Task">the id and the alias of the engineer's task </param>

public class Engineer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public EngineerExperience Level { get; set; }
    public double SaleryPerHour { get; set; }
    public string? Email { get; set; }
    public TaskInEngineer? Task { get; set; }
    public override string ToString()
    {
        return "ID: " + Id + "\nName: " + Name + "\nEmail: " + Email + "\nLevel: " + Level + "\nSalary Per Hour: " + SaleryPerHour + "\nEnginner's Task: " + ((Task != null) ? Task!.ToString() : "no task\n");
    }
}



