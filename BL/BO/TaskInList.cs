
using System.Reflection.Emit;
using System.Xml.Linq;

namespace BO;
/// <summary>
/// TaskInList entity which describes an already existing task
/// </summary>
/// <param name="Id">the id of the task</param>
/// <param name="TaskNickname">the task's alias</param>
/// <param name="Description">the task's description</param>
/// <param name="Status">the task's status</param>
public class TaskInList
{
    public int Id { get; set; }
    public string? TaskNickname { get; set; }
    public string Description { get; set; }
    public Status Status { get; set; }
    public override string ToString()
    {
        return "ID: " + Id + "\nAlias: " + TaskNickname + "\nDescription: " + Description + "\nStatus: " + Status + "\n";
    }
}
