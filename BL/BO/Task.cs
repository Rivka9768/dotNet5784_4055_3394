
namespace BO;
/// <summary>
/// Task entity which repesents a task and its properties.
/// </summary>
/// <param name="Id">the task's id</param>
/// <param name="TaskNickname">the task's nickname</param
/// <param name="Description">the description of the task</param>
/// <param name="ProductionDate">the date the task was created</param>
/// <param name="Status">the status of the task</param>
/// <param name="DependenciesList">the tasks who are dependent on this task</param>
/// <param name="Milestone">the id and alias of the milestone in the task</param>
/// <param name="EstimatedStartDate">estimated date to start the task</param>
/// <param name="ActualStartDate">the start date of the task</param>
///  <param name="EstimatedEndDate">estimated date to complete the task</param>
/// <param name="Deadline">the deadline for completing the task</param>
/// <param name="ActualEndDate">the actual date of completing the task</param>
/// <param name="Products">the products of the task</param>
/// <param name="Remarks">remarks on the task</param>
/// <param name="Engineer">the id and name of the engineer dealing with the task</param>
/// <param name="Difficulty">the task's level of difficulty</param>
public class Task
{
    public int Id { get; init; }
    public string? TaskNickname { get;set; }
    public string Description { get; set; }
    public DateTime ProductionDate { get; set; }
    public Status Status { get; set; }
    public List<TaskInList>? DependenciesList { get; set; }
    public MilestoneInTask? Milestone { get; set; }
    public DateTime? EstimatedStartDate { get; set; }
    public DateTime? ActualStartDate { get; set; }
    public DateTime? EstimatedEndDate { get; set; }
    public DateTime Deadline { get; set; }
    public DateTime? ActualEndDate { get; set; }
    public string? Products { set; get; }
    public string? Remarks { set; get; }
    public EngineerInTask? Engineer { set; get; }
    public EngineerExperience Difficulty { set; get; }  
}
