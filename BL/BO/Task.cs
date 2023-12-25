
namespace BO;

internal class Task
{
    public int Id { get; init; }
    public string? TaskNickname { get;set; }
    public string Description { get; set; }
    public DateTime ProductionDate { get; set; }
    public Status Status { get; set; }
    public List<TaskInList> DependenciesList { get; set; }
    public MilestoneInTask Milestone { get; set; }
    public DateTime? EstimatedStartDate { get; set; }
    public DateTime? ActualStartDate { get; set; }
    public DateTime? EstimatedEndDate { get; set; }
    public DateTime Deadline { get; set; }
    public DateTime? ActualEndDate { get; set; }
    public string? Products { set; get; }
    public string? Remarks { set; get; }
    public int? EngineerId { set; get; }
    public EngineerExperience Difficulty { set; get; }  
}
