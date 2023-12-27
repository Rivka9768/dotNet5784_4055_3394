
using BlApi;

using BO;
using System.Text.RegularExpressions;

namespace BlImplementation;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    public static bool IsValidEmail(this string email)
    {
        string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
        var regex = new Regex(pattern, RegexOptions.IgnoreCase);
        return regex.IsMatch(email);
    }
    public void Create(Engineer engineer)
    {

        if (engineer.Id <= 0 && engineer.Id > 999999999)
            throw new Exception();
        if (engineer.Name == "")
            throw new Exception();
        if (engineer.SaleryPerHour <= 0)
            throw new Exception();
        if (engineer.Email != "" && !IsValidEmail(engineer.Email))
            throw new Exception();
        DO.Engineer newEngineer = new(engineer.Id, engineer.Name, (DO.EngineerExperience)(int)engineer.Level, engineer.SaleryPerHour, engineer.Email);
        try
        {
            _dal.Engineer.Create(newEngineer);
        }catch (Exception e)
        {
            throw new Exception();
        }
    }




    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Engineer? Read(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Engineer> ReadAll(Func<Engineer, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(Engineer engineer)
    {
        throw new NotImplementedException();
    }
}
