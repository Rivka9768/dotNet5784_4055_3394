

namespace BO;
/// <summary>
/// the Milestone entity and it's properties
/// </summary>
/// <param name="Id">the id of the milestone</param>
/// <param name="MilestoneNickname">the alias of the milestone</param>
/// <param name="Description">the description of the milestone</param>
/// <param name="ProductionDate">the date the milestone was created</param>
/// <param name="Status">the status of the milestone</param>
/// <param name="StartDate">the date to start the milestone</param>
///  <param name="EstimatedEndDate">estimated date to complete the milestone</param>
/// <param name="Deadline">the deadline for completing the milestone</param>
/// <param name="ActualEndDate">the actual date of completing the milestone</param>
/// <param name="progressPercentage">the progress percentage of the milestone</param>
/// <param name="Remarks">remarks on the milestone</param>
/// <param name="DependenciesList">the tasks who are dependent on this milestone</param>
public class Milestone
{
    public int Id { get; init; }
    public string? MilestoneNickname { get; set; }
    public string Description { get; set; }
    public DateTime ProductionDate { get; set; }
    public Status Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EstimatedEndDate { get; set; }
    public DateTime Deadline { get; set; }
    public DateTime? ActualEndDate { get; set; }
    public double progressPercentage { get; set; }  
    public string? Remarks { set; get; }
    public List<TaskInList>? DependenciesList { get; set; }
}
