
namespace BO;
/// <summary>
/// MilestoneInTask entity which describes a milestone in a task
/// </summary>
/// <param name="Id">the milestone's id</param>
/// <param name="MilestoneNickname">the milestone's alias</param>
public class MilestoneInTask
{
    public int Id { get; set; }
    public string? MilestoneNickname { get; set; }
}
