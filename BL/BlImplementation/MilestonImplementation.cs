

using BlApi;
using BO;

namespace BlImplementation;

internal class MilestonImplementation : IMilestone
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    /// <summary>
    /// claculates the status of the milestone according to it's start date,final date and deadline.
    /// </summary>
    /// <param name="task"></param>
    /// <returns> status </returns>
    private static Status GetStatus(DO.Task task)
    {
        Status status = Status.Unscheduled;
        if (task.StartDate != null)
        {
            if (task.FinalDate != null && (((DateTime)task.FinalDate).AddDays(4) >= task.Deadline))
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

    /// <summary>
    /// calculates the progress percentage of the tasks in the milestone.
    /// </summary>
    /// <param name="taskInList"></param>
    /// <returns>progress percentage </returns>
    private double CalcProgressPercentage(List<TaskInList>? taskInList)
    {
        if(taskInList==null)
            return 100;
        List < DO.Task > completedTasks= (from t in taskInList
                           where _dal.Task.Read(t.Id)!.FinalDate > DateTime.MinValue
                           select _dal.Task.Read(t.Id)).ToList();
        return (double)(completedTasks.Count / taskInList.Count) * 100;
    }

    /// <summary>
    /// returns a milestone with the Id given.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
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

    /// <summary>
    /// updates detailes of an already existing milestone
    /// </summary>
    /// <param name="milestone"></param>
    /// <returns> updated milestone </returns>
    /// <exception cref="BO.BlNullPropertyException"></exception>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public Milestone Update(Milestone milestone)
    {
        if (milestone == null)
            throw new BO.BlNullPropertyException("can not update a null milestone");
        DO.Task? task = _dal.Task.Read(milestone.Id);
        if (task == null || task.Milestone == false)
            throw new BO.BlDoesNotExistException($"milestone with Id={milestone.Id} does not exist");

        /* DO.Task? updatedTask = new DO.Task { Description = (milestone.Description != null) ? milestone.Description : task.Description,
             TaskNickname = (milestone.MilestoneNickname != null) ? milestone.MilestoneNickname : task.TaskNickname, 
             Remarks = (milestone.Remarks != null) ? milestone.Remarks : task.Remarks };*/

        DO.Task? updatedTask = new(task.Id, (milestone.Description != null) ? milestone.Description : task.Description, task.ProductionDate, task.Deadline, task.Difficulty
            , task.EngineerId, task.Milestone, task.Duration,task.EstimatedStartDate, task.StartDate, task.EstimatedEndDate, task.FinalDate, (milestone.MilestoneNickname != null) ? milestone.MilestoneNickname : task.TaskNickname
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
