
using System.Xml.Linq;

namespace BO;
/// <summary>
/// TaskInEngineer entity which describes tasks related to a specific engineer 
/// </summary>
/// <param name="Id">task's id</param>
/// <param name="TaskNickname">task's alias</param>
public class TaskInEngineer
{
    public int Id { get; set; }
    public string? TaskNickname { get; set; }
    public override string ToString()
    {
        return "\tID: " + Id 
            + "\tAlias " + TaskNickname+"\n";
    }
}
