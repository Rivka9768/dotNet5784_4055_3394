

namespace BO;

public class EngineerInList
{
    public int Id { get; set; }
    public string Name { get; set; }
    public EngineerExperience Level { get; set; }
    public override string ToString()
    {
        return "ID: " + Id + "\nName: " + Name + "\nLevel: " + Level + "\n";
    }
}
