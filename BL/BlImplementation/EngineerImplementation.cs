
using BlApi;
using BO;
using System.Text.RegularExpressions;

namespace BlImplementation;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    private bool IsValidEmail(string email)
    {
        string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
        var regex = new Regex(pattern, RegexOptions.IgnoreCase);
        return regex.IsMatch(email);
    }
    private bool ValidateInput(Engineer engineer)
    {
        if (engineer.Id <= 0 && engineer.Id > 999999999)
            return false;
        if (engineer.Name == "")
            return false;
        if (engineer.SaleryPerHour <= 0)
            return false;
        if (engineer.Email != "" && !IsValidEmail(engineer.Email))
            return false;
        return true;
    }
    private TaskInEngineer? ReadTaskInEngineer(int id)
    {
        List<DO.Task?> tasks = _dal.Task.ReadAll().ToList();
        TaskInEngineer? taskInEngineer = (from t in tasks
                                          let engineerId = t.EngineerId
                                          where engineerId == id
                                          select new TaskInEngineer { Id = id, TaskNickname = t.TaskNickname }).FirstOrDefault();
        return taskInEngineer;
    }
    public void Create(BO.Engineer engineer)
    {
        if (!ValidateInput(engineer))
            throw new Exception();
        DO.Engineer newEngineer = new(engineer.Id, engineer.Name, (DO.EngineerExperience)(int)engineer.Level, engineer.SaleryPerHour, engineer.Email);
        try
        {
            _dal.Engineer.Create(newEngineer);
        }
        catch (Exception e)
        {
            throw new Exception();
        }
    }




    public void Delete(int id)
    {
        TaskInEngineer? taskInEngineer = ReadTaskInEngineer(id);
        if (taskInEngineer != null)
        {
            DO.Task? task = _dal.Task.Read(taskInEngineer!.Id);
                if (task != null&&(task.FinalDate <= DateTime.Now || task.StartDate <= DateTime.Now))
                    throw new Exception();
        }
        try
        {
            _dal.Engineer.Delete(id);
        }
        catch (Exception e)
        {
            throw new Exception();
        }
    }

    public Engineer? Read(int id)
    {
        //למצא בבית את ה read 
        throw new NotImplementedException();
    }


    public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool>? filter = null)
    {
        return (from DO.Engineer doEngineer in _dal.Engineer.ReadAll((Func<DO.Engineer, bool>?)filter).ToList()
                select new BO.Engineer
                {
                    Id = doEngineer.Id,
                    Name = doEngineer.Name,
                    Email = doEngineer.Email,
                    Level = (BO.EngineerExperience)(int)doEngineer.Level,
                    SaleryPerHour = doEngineer.SaleryPerHour,
                    Task = ReadTaskInEngineer(doEngineer.Id)

                });
    }

    public void Update(Engineer engineer)
    {
        if (!ValidateInput(engineer))
            throw new Exception();
        DO.Engineer newEngineer = new(engineer.Id, engineer.Name, (DO.EngineerExperience)(int)engineer.Level, engineer.SaleryPerHour, engineer.Email);
        try
        {
            _dal.Engineer.Update(newEngineer);
        }
        catch (Exception e)
        {
            throw new Exception();
        }
    }
}
