

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
        if (task == null || task.Milestone == false)
            throw new BO.BlDoesNotExistException($"milestone with Id={id} does not exist");
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

    //האם הupdate אמור לקבל רק  id?
    public Milestone Update(Milestone milestone)
    {
        if (milestone == null)
            throw new BO.BlNullPropertyException("can not update a null milestone");
        DO.Task? task = _dal.Task.Read(milestone.Id);
        if (task == null || task.Milestone == false)
            throw new BO.BlDoesNotExistException($"milestone with Id={milestone.Id} does not exist");
        //האם זה תקין???
        /* DO.Task? updatedTask = new DO.Task { Description = (milestone.Description != null) ? milestone.Description : task.Description,
             TaskNickname = (milestone.MilestoneNickname != null) ? milestone.MilestoneNickname : task.TaskNickname, 
             Remarks = (milestone.Remarks != null) ? milestone.Remarks : task.Remarks };*/
        //האם יש דרך יותר סבירה לעדכן רק כמה שדות???
        DO.Task? updatedTask = new(task.Id, (milestone.Description != null) ? milestone.Description : task.Description, task.ProductionDate, task.Deadline, task.Difficulty
            , task.EngineerId, task.Milestone, task.Duration
            , task.EstimatedStartDate, task.StartDate, task.EstimatedEndDate, task.FinalDate, (milestone.MilestoneNickname != null) ? milestone.MilestoneNickname : task.TaskNickname
            , (milestone.Remarks != null) ? milestone.Remarks : task.Remarks, task.Products);
        try
        {
            _dal.Task.Update(updatedTask);
        }catch(DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"milestone with Id={milestone.Id} does not exist",ex);
        }
        return milestone;
    }
}
