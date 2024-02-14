

using BlApi;
using BO;

namespace BlImplementation;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    /// <summary>
    /// claculates the status of the task according to it's start date,final date and deadline.
    /// </summary>
    /// <param name="task"></param>
    /// <returns> status of task </returns>
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
    /// validates task detailes coming from the user's input
    /// </summary>
    /// <param name="task"></param>
    /// <exception cref="BlInValidInput"></exception>
    private void ValidateInput(BO.Task task)
    {
        if (task.Description == null)
            throw new BlInValidInput("task's description can't be empty");
        if (task.ProductionDate == DateTime.MinValue || ((task.ActualStartDate != null) && task.ProductionDate > task.ActualStartDate) || ((task.EstimatedStartDate != null) && task.ProductionDate > task.EstimatedStartDate) || ((task.ActualEndDate != null) && task.ProductionDate > task.ActualEndDate) || ((task.EstimatedEndDate != null) && task.ProductionDate > task.EstimatedEndDate) || task.ProductionDate > task.Deadline)
            throw new BlInValidInput("task's production date can only be after be 01/01/0001 and has to be before estimated/actual atart date, estimated/actual end date and before deadline");
        if (task.Deadline == DateTime.MinValue || ((task.ActualStartDate != null) && task.Deadline < task.ActualStartDate) || ((task.EstimatedStartDate != null) && task.Deadline < task.EstimatedStartDate) || ((task.ActualEndDate != null) && task.Deadline < task.ActualEndDate) || ((task.EstimatedEndDate != null) && task.Deadline < task.EstimatedEndDate) || task.Deadline < task.ProductionDate)
            throw new BlInValidInput("task's deadline can only be after be 01/01/0001 and has to be after estimated/actual end date, estimated/actual start date and after production date");
        if (task.EstimatedStartDate != null && (((task.EstimatedEndDate != null) && task.EstimatedStartDate > task.EstimatedEndDate) || ((task.ActualEndDate != null) && task.EstimatedStartDate > task.ActualEndDate)))
            throw new BlInValidInput("task's estimated start date can't be after task's estimated end date or actual end date");
        if (task.ActualStartDate != null && (((task.EstimatedEndDate != null) && task.ActualStartDate > task.EstimatedEndDate) || ((task.ActualEndDate != null) && task.ActualStartDate > task.ActualEndDate)))
            throw new BlInValidInput("task's actual start date can't be after task's estimated end date or actual end date");
        if (task.ActualEndDate != null && (((task.EstimatedStartDate != null) && task.ActualEndDate < task.EstimatedStartDate) || ((task.ActualStartDate != null) && task.ActualEndDate < task.ActualStartDate)))
            throw new BlInValidInput("task's estimated end date can't be before task's estimated start date or actual start date");
        if (task.EstimatedEndDate != null && (((task.EstimatedStartDate != null) && task.EstimatedEndDate < task.EstimatedStartDate) || ((task.ActualStartDate != null) && task.EstimatedEndDate < task.ActualStartDate)))
            throw new BlInValidInput("task's actual end date can't be before task's estimated start date or actual start date");
        if (task.Engineer != null && (_dal.Engineer.Read(task.Engineer.Id) == null || _dal.Engineer.Read(task.Engineer.Id)!.Name != task.Engineer.Name))
            throw new BlInValidInput("invalid engineer detailes");
        if (task.Difficulty == EngineerExperience.All)
            throw new BlInValidInput("can't choose level 'All' as task's difficulty");
        if (task.Engineer != null && _dal.Engineer.Read(task.Engineer.Id)!.Level < (DO.EngineerExperience)(int)task.Difficulty)
            throw new BlInValidInput("invalid engineer detailes");
        return;
    }

    /// <summary>
    /// adds a new task.
    /// </summary>
    /// <param name="task"></param>
    public void Create(BO.Task task)
    {
        ValidateInput(task);
        DO.Task doTask = new(task.Id, task.Description, task.ProductionDate, task.Deadline, (DO.EngineerExperience)(int)task.Difficulty
            , task.Engineer?.Id, (task.Milestone) != null ? true : false, (task.ActualEndDate - task.ActualStartDate)
            , task.EstimatedStartDate, task.ActualStartDate, task.EstimatedEndDate, task.ActualEndDate, task.TaskNickname
            , task.Remarks, task.Products);
        _dal.Task.Create(doTask);
        while (task.DependenciesList != null)
        {
            DO.Dependency dependency = new(0, task.DependenciesList.First().Id, task.Id);
            _dal.Dependency.Create(dependency);
            task.DependenciesList.RemoveAt(0);
        }
    }

    /// <summary>
    /// deletes a task by it's ID.
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    /// <exception cref="BlDeletionImpossible"></exception>
    public void Delete(int id)
    {
        if (_dal.Task.Read(id) == null)
            throw new BO.BlDoesNotExistException($"Task with ID={id} does not exists");
        if (_dal.Dependency.ReadAll().ToList().Find(d => d.IdDependantTask == id) != null)
            throw new BlDeletionImpossible($"can not delete task with Id={id}");
        try
        {
            _dal.Task.Delete(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with ID={id} does not exists", ex);
        }
    }

    /// <summary>
    /// returns a task with the Id given.
    /// </summary>
    /// <param name="id"></param>
    /// <returns> task </returns>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public BO.Task? Read(int id)
    {
        DO.Task? task = _dal.Task.Read(id);
        if (task == null)
            throw new BO.BlDoesNotExistException($"Task with ID={id} does not exists");
        Status status = GetStatus(task);
        //not nedded calculations was done as bonus
        MilestoneInTask milestoneInTask = (from d in _dal.Dependency.ReadAll()
                                           let IdPreviousTask = d.IdPreviousTask
                                           where IdPreviousTask == id && (_dal.Task.Read(d.IdDependantTask)!.Milestone == true)
                                           select new MilestoneInTask { Id = d.IdDependantTask, MilestoneNickname = _dal.Task.Read(d.IdDependantTask)!.TaskNickname }).FirstOrDefault()!;
        //not nedded calculations was done as bonus
        List<TaskInList>? DependenciesList = (from d in _dal.Dependency.ReadAll()
                                              let IdDependantTask = d.IdDependantTask
                                              where IdDependantTask == id
                                              select new TaskInList { Id = d.IdPreviousTask, TaskNickname = _dal.Task.Read(d.IdPreviousTask)!.TaskNickname, Description = _dal.Task.Read(d.IdPreviousTask)!.Description, Status = GetStatus(_dal.Task.Read(d.IdPreviousTask)) }).ToList();
        EngineerInTask? engineerInTask = (task.EngineerId) != null ? (new EngineerInTask { Id = (int)task.EngineerId, Name = _dal.Engineer.Read((int)task.EngineerId)!.Name }) : null;
        return new BO.Task
        {
            Id = task.Id,
            TaskNickname = task.TaskNickname,
            Description = task.Description,
            ProductionDate = task.ProductionDate,
            Status = status,
            DependenciesList = DependenciesList,
            Milestone = milestoneInTask,
            EstimatedStartDate = task.EstimatedStartDate,
            ActualStartDate = task.StartDate,
            EstimatedEndDate = task.EstimatedEndDate,
            Deadline = task.Deadline,
            ActualEndDate = task.FinalDate,
            Products = task.Products,
            Remarks = task.Remarks,
            Engineer = engineerInTask,
            Difficulty = (BO.EngineerExperience)(int)task.Difficulty,
        };
    }

    /// <summary>
    /// returns a list of tasks  which matches the filter if givan in the format of the entity TaskInList.
    /// </summary>
    /// <param name="filter"></param>
    /// <returns> a list of TaskInList </returns>
    public IEnumerable<BO.TaskInList?> ReadAll(Func<BO.Task, bool>? filter = null)
    {
        List<BO.Task> boTasks = (from DO.Task doTask in _dal.Task.ReadAll()
                                 select new BO.Task
                                 {
                                     Id = doTask.Id,
                                     TaskNickname = doTask.TaskNickname,
                                     Description = doTask.Description,
                                     ProductionDate = doTask.ProductionDate,
                                     Status = GetStatus(doTask),
                                     DependenciesList = null,
                                     Milestone = null,
                                     EstimatedStartDate = doTask.EstimatedStartDate,
                                     ActualStartDate = doTask.StartDate,
                                     EstimatedEndDate = doTask.EstimatedEndDate,
                                     Deadline = doTask.Deadline,
                                     ActualEndDate = doTask.FinalDate,
                                     Products = doTask.Products,
                                     Remarks = doTask.Remarks,
                                     Engineer = null,
                                     Difficulty = (BO.EngineerExperience)(int)doTask.Difficulty,
                                 }).ToList();
        return (filter != null) ? boTasks.Where(bTask => filter!(bTask)).Select(bTask => new BO.TaskInList { Id = bTask.Id, TaskNickname = bTask.TaskNickname, Description = bTask.Description, Status = bTask.Status }).ToList()
             : boTasks.Select(bTask => new BO.TaskInList { Id = bTask.Id, TaskNickname = bTask.TaskNickname, Description = bTask.Description, Status = bTask.Status }).ToList();
    }

    /// <summary>
    /// updates detailes of an already existing task
    /// </summary>
    /// <param name="task"></param>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void Update(BO.Task task)
    {
        ValidateInput(task);
        DO.Task doTask = new(task.Id, task.Description, task.ProductionDate,
             task.Deadline, (DO.EngineerExperience)(int)task.Difficulty, task.Engineer?.Id, (task.Milestone) != null ? true : false, (task.ActualEndDate - task.ActualStartDate)
           , task.EstimatedStartDate, task.ActualStartDate, task.EstimatedEndDate, task.ActualEndDate
           , task.TaskNickname, task.Remarks, task.Products);
        try
        {
            _dal.Task.Update(doTask);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with ID={task.Id} does not exists", ex);

        }
    }
}
