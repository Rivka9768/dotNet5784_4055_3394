
namespace BO;
/// <summary>
/// MilestoneInList entity which describes an already existing milestone
/// </summary>
/// <param name="Id">the id of the milestone</param>
/// <param name="MilestoneNickname">the milestone's alias</param>
/// <param name="Description">the milestone's description</param>
/// <param name="Status">the milestone's status</param>
/// <param name="progressPercentage">the progress percentage of the milestone</param>
public class MilestoneInList
{
    public int Id { get; set; }
    public string? MilestoneNickname { get; set; }
    public string Description { get; set; }
    public Status Status { get; set; }
    public double progressPercentage { get; set; }
}
