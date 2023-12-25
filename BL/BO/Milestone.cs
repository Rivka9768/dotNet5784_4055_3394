

namespace BO;

internal class Milestone
{
    public int Id { get; set; }
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
    public List<TaskInList> DependenciesList { get; set; }
}
