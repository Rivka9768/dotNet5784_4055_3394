

using BlApi;
using BO;

namespace BlImplementation;

internal class TaskImplementation : ITask
{
    //האם duration של משימה הינו לפי הstart וend האמיתי או המשוער?????
    private DalApi.IDal _dal = DalApi.Factory.Get;

    private static Status GetStatus(DO.Task task)
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
    public void Create(BO.Task task)
    { //למה צריך לבדוק id זה בכלל מספר רץ
        if (task.Id <= 0)
            //לשאול את המורה לגבי ה כינוי של ה task
            throw new BlInValidInput("the details are invalid");
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

    public BO.Task? Read(int id)
    {
        DO.Task? task = _dal.Task.Read(id);
        if (task == null)
            throw new BO.BlDoesNotExistException($"Task with ID={id} does not exists");
        Status status = GetStatus(task);
        MilestoneInTask milestoneInTask = (from d in _dal.Dependency.ReadAll()
                                           let IdPreviousTask = d.IdPreviousTask
                                           where IdPreviousTask == id && (_dal.Task.Read(d.IdDependantTask)!.Milestone == true)
                                           select new MilestoneInTask { Id = d.IdDependantTask, MilestoneNickname = _dal.Task.Read(d.IdDependantTask)!.TaskNickname }).FirstOrDefault()!;
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
            //מה עם משך הזמן של המשימה???
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

    /*    public IEnumerable<BO.Task?> ReadAll(Func<BO.Task, bool>? filter = null)
        {
            return (from DO.Task doTask in _dal.Task.ReadAll((Func<DO.Task, bool>?)filter).ToList()
                    select Read(doTask.Id));
        }*/
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
                                     //מה עם משך הזמן של המשימה???
                                     ActualStartDate = doTask.StartDate,
                                     EstimatedEndDate = doTask.EstimatedEndDate,
                                     Deadline = doTask.Deadline,
                                     ActualEndDate = doTask.FinalDate,
                                     Products = doTask.Products,
                                     Remarks = doTask.Remarks,
                                     Engineer = null,
                                     Difficulty = (BO.EngineerExperience)(int)doTask.Difficulty,
                                 }).ToList();
       return (filter != null) ? boTasks.Where(bTask => filter!(bTask)).Select(bTask=>new BO.TaskInList { Id = bTask.Id, TaskNickname = bTask.TaskNickname, Description = bTask.Description, Status = bTask.Status }).ToList()
            : boTasks.Select(bTask=>new BO.TaskInList { Id = bTask.Id, TaskNickname = bTask.TaskNickname, Description = bTask.Description, Status = bTask.Status }).ToList();
    }
    public void Update(BO.Task task)
    {
        //נתונים ibput לעשות וולידצית 
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

        // task.DependenciesList
        //האם להתיחס למערך התלויות
    }
}
