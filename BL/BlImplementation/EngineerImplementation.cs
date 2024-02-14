
using BlApi;
using BO;

using System.Text.RegularExpressions;

namespace BlImplementation;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    /// <summary>
    /// checks if engineer's email address is legal.
    /// </summary>
    /// <param name="email"></param>
    /// <returns> true if the email is legal and false otherwise </returns>
    private bool IsValidEmail(string email)
    {
        string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
        var regex = new Regex(pattern, RegexOptions.IgnoreCase);
        return regex.IsMatch(email);
    }

    /// <summary>
    /// validates engineer's detailes coming from the user's input
    /// </summary>
    /// <param name="engineer"></param>
    /// <exception cref="BlInValidInput"></exception>
    private void ValidateInput(Engineer engineer)
    {
        if (engineer.Id <= 0 && engineer.Id > 999999999)
            throw new BlInValidInput("invalid ID");
        if (engineer.Name == null)
            throw new BlInValidInput("engineer's name can't be empty");
        if (engineer.SaleryPerHour <= 0)
            throw new BlInValidInput("engineer's salary is illegal");
        if ((engineer.Email != null) && !IsValidEmail(engineer.Email))
            throw new BlInValidInput("engineer's email address is illegal");
        if (engineer.Level == EngineerExperience.All)
            throw new BlInValidInput("can't choose level 'All' as engineer's level");
        List<DO.Task> tasks = _dal.Task.ReadAll(task => (task.EngineerId != null) && task.EngineerId == engineer.Id).Where(task => task!.Difficulty > (DO.EngineerExperience)(int)engineer.Level).ToList()!;
        if (tasks.Count!=0)
            throw new BlInValidInput("invalid engineer level detailes");
        return ;
    }
    
    /// <summary>
    /// calculates the current task of the engineer.
    /// </summary>
    /// <param name="id"></param>
    /// <returns> engineer's current task </returns>
    private TaskInEngineer? ReadTaskInEngineer(int id)
    {
        List<DO.Task?> tasks = _dal.Task.ReadAll().ToList();
        TaskInEngineer? taskInEngineer = (from t in tasks
                                          let engineerId = t.EngineerId 
                                          where engineerId == id && t.FinalDate>DateTime.Now
                                          select new TaskInEngineer { Id = t.Id, TaskNickname = t.TaskNickname }).FirstOrDefault();
        return taskInEngineer;
    }
    
    /// <summary>
    /// adds a new engineer
    /// </summary>
    /// <param name="engineer"></param>
    /// <exception cref="BO.BlAlreadyExistsException"></exception>
    public void Create(BO.Engineer engineer)
    {
        ValidateInput(engineer);
        DO.Engineer newEngineer = new(engineer.Id, engineer.Name, (DO.EngineerExperience)(int)engineer.Level, engineer.SaleryPerHour, engineer.Email);
        try
        {
            _dal.Engineer.Create(newEngineer);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Engineer with ID={engineer.Id} already exists", ex);
        }

    }

    /// <summary>
    /// deletes an engineer by it's Id.
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="BlDeletionImpossible"></exception>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void Delete(int id)
    {
        TaskInEngineer? taskInEngineer = ReadTaskInEngineer(id);
        if (taskInEngineer != null)
        {
            DO.Task? task = _dal.Task.Read(taskInEngineer!.Id);
            if (task != null && (task.FinalDate <= DateTime.Now || task.StartDate <= DateTime.Now))
                throw new BlDeletionImpossible($"can not delete engineer with Id={id}");
        }
        try
        {
            _dal.Engineer.Delete(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Engineer with ID={id} does not exists", ex);
        }
    }

    /// <summary>
    /// returns the engineer with the Id given.
    /// </summary>
    /// <param name="id"></param>
    /// <returns> engineer </returns>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public BO.Engineer? Read(int id)
    {
        DO.Engineer? engineer = _dal.Engineer.Read(id);
        if (engineer == null)
            throw new BO.BlDoesNotExistException($"Engineer with ID={id} does not exists");
        DO.Task? task = _dal.Task.Read(task => (task.EngineerId == id));
        BO.TaskInEngineer? taskInEngineer = null;
        if (task != null)
           taskInEngineer = new BO.TaskInEngineer { Id = task.Id, TaskNickname = task.TaskNickname };
        return new BO.Engineer { Id = engineer.Id, Name = engineer.Name, Level = (BO.EngineerExperience)(int)engineer.Level, SaleryPerHour = engineer.SaleryPerHour, Email = engineer.Email, Task = taskInEngineer };
    }

    /// <summary>
    /// returns a list of engineers which matches the filter if givan in the format of the entity EngineerInList.
    /// </summary>
    /// <param name="filter"></param>
    /// <returns> a list of EngineerInList</returns>
    public IEnumerable<BO.EngineerInList> ReadAll(Func<BO.Engineer, bool>? filter = null)
    {
        List<BO.Engineer> boEngineers= (from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
                                                                   select new BO.Engineer
                                                                   {
                                                                       Id = doEngineer.Id,
                                                                       Name = doEngineer.Name,
                                                                       Email = doEngineer.Email,
                                                                       Level = (BO.EngineerExperience)(int)doEngineer.Level,
                                                                       SaleryPerHour = doEngineer.SaleryPerHour,
                                                                       Task = ReadTaskInEngineer(doEngineer.Id)

                                                                   }).ToList();
        return (filter != null) ? boEngineers.Where(bEngineer => filter!(bEngineer)).Select(bEngineer => new BO.EngineerInList { Id = bEngineer.Id, Name = bEngineer.Name, Level = bEngineer.Level }).ToList()
            : boEngineers.Select(bEngineer => new BO.EngineerInList { Id = bEngineer.Id, Name = bEngineer.Name, Level = bEngineer.Level }).ToList();
    }

    /// <summary>
    /// updates detailes of an already existing engineer
    /// </summary>
    /// <param name="engineer"></param>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void Update(Engineer engineer)
    {
        ValidateInput(engineer);
        DO.Engineer newEngineer = new(engineer.Id, engineer.Name, (DO.EngineerExperience)(int)engineer.Level, engineer.SaleryPerHour, engineer.Email);
        try
        {
            _dal.Engineer.Update(newEngineer);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Engineer with ID={engineer.Id} does not exists", ex);

        }
    }
}