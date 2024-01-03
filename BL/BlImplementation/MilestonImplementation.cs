

using BlApi;
using BO;

namespace BlImplementation;

internal class MilestonImplementation : IMilestone
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    private Status GetStatus(DO.Task task)
    {
        Status status = Status.Unscheduled;
        if (task.StartDate != null)
        {
            //לשאול את המורה מזה אומר בסיכון
            if (task.FinalDate > task.Deadline || DateTime.Now > task.Deadline)
                status = Status.InJeopardy;
            else
            {
                if (task.StartDate > DateTime.Now)
                    status = Status.Scheduled;
                else
                    status = Status.OnTrack;
            }
        }
        return status;
    }

    private double CalcProgressPercentage(List<TaskInList>? taskInList)
    {
        if(taskInList==null)
            return 100;
        List < DO.Task > completedTasks= (from t in taskInList
                           where _dal.Task.Read(t.Id)!.FinalDate > DateTime.MinValue
                           select _dal.Task.Read(t.Id)).ToList();
        return (double)(completedTasks.Count / taskInList.Count) * 100;
    }
    public Milestone? Read(int id)
    {
        DO.Task? task = _dal.Task.Read(id);
        if (task == null || task.Milestone==false)
            throw new Exception();
        List<TaskInList>? taskInList = (from d in _dal.Dependency.ReadAll()
                                           let IdDependantTask = d.IdDependantTask
                                           where IdDependantTask == id
                                           let prevTask = _dal.Task.Read(d.IdPreviousTask)
                                           select new TaskInList { Id = d.IdPreviousTask, TaskNickname = prevTask!.TaskNickname, Description = prevTask!.Description,Status=GetStatus(prevTask) }).ToList();
        //איך באמת מחשבים אחוז התקדמות???
        return new BO.Milestone
        {
            Id = id,
            MilestoneNickname = task.TaskNickname,
            Description = task.Description,
            ProductionDate = task.ProductionDate,
            Status = GetStatus(task),
            StartDate = task.StartDate,
            EstimatedEndDate = task.EstimatedEndDate,
            Deadline = task.Deadline,
            ActualEndDate = task.FinalDate,
            ProgressPercentage = CalcProgressPercentage(taskInList),
            Remarks = task.Remarks,
            DependenciesList = taskInList
        };
}

    public Milestone Update(Milestone milestone)
    {
        
    }
}
