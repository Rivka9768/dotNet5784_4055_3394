

namespace DalTest;
using DalApi;
using DO;
using static System.Runtime.InteropServices.JavaScript.JSType;

public static class Initialization
{

    private static IDal? s_dal;  

    private static readonly Random s_rand = new();

    /// <summary>
    /// this function initializes engineers
    /// </summary>
    private static void CreateEngineers()
    {
        string[] engineerNames =
        {
            "Rivka Sorscher", "Leah Shitrit", "Moshe Sharet",
            "Binyamin Netanyahu", "Yoav Galant", "Golda Meir","Levi Eshkol"
        };
        string[] engineerEmails =
{
            "rivkasorscher@gmail.com", "leahshitrit@gmail.com", "moshesharet@prime.ministry.gov.il",
            "bbnetanyahu@prime.ministry.gov.il", "yoavgalant@security.ministry.gov.il",
            "goldameir@prime.ministry.gov.il","levieshkol@prime.ministry.gov.il"
        };
        int index = -1;
        foreach (string _name in engineerNames)
        {
            int _id;
            do
                _id = s_rand.Next(100000000, 1000000000);
            while (s_dal!.Engineer.Read(_id) != null);
            string _email = engineerEmails[index+=1];
            EngineerExperience _level = (EngineerExperience)s_rand.Next(0, 5);
            double _saleryPerHour = 100 * ((int)_level+1);
            Engineer newEngineer = new(_id, _name, _level, _saleryPerHour, _email);
            s_dal!.Engineer.Create(newEngineer);
        }
    }

    /// <summary>
    /// this function initializes tasks
    /// </summary>
    private static void CreateTasks()
    {
        int _id = 0;
        bool _milestone = false;
        string _description = "no description yet";
        for (int i = 0; i < 20; i++)
        {
            DateTime date= DateTime.Now;
            TimeSpan timeSpan = date.AddDays(30) - date;
            TimeSpan newSpan = new TimeSpan(0, s_rand.Next(0, (int)timeSpan.TotalMinutes), 0);
            DateTime _productionDate = date + newSpan;
            DateTime _deadline= _productionDate.AddDays(s_rand.Next(30,100));
            List<Engineer> engineers= s_dal!.Engineer.ReadAll().ToList()!;
            EngineerExperience _difficulty = (EngineerExperience)s_rand.Next(0, 5);
            List<Engineer> qualifiedEngineers = engineers.FindAll((e) => e.Level >= _difficulty);
            int amountEngineers = qualifiedEngineers.Count;
            int? _engineerId = null;
            if (amountEngineers!=0)
            {
                int randomEngineerNum = s_rand.Next(0, amountEngineers);
                _engineerId = qualifiedEngineers[randomEngineerNum].Id;
            }
            Task newTask = new(_id, _description, _productionDate, _deadline, _difficulty, _engineerId, _milestone);
            s_dal!.Task.Create(newTask);
        }
    }

    /// <summary>
    /// this function initializes dependencies
    /// </summary>
    private static void CreateDependencys()
    {
        int _id = 0;
        int _idPreviousTask;
        int _idDependantTask;
        for (int i = 0; i < 40; i++) 
        {
            List<Dependency> dependencies = s_dal!.Dependency.ReadAll().ToList()!;
            do
            {
                _idPreviousTask = s_rand.Next(1, 16);
                _idDependantTask = s_rand.Next(_idPreviousTask + 1, 17);
            } while (dependencies.Find((d) =>
                (d.IdPreviousTask == _idPreviousTask &&d.IdDependantTask== _idDependantTask)) != null);
            Dependency newDependency = new(_id, _idPreviousTask, _idDependantTask);
            s_dal!.Dependency.Create(newDependency);
        }
    }
    


    public static void Do(IDal dal)
    {
        s_dal = dal ?? throw new NullReferenceException("DAL can not be null!");
        CreateDependencys();
        CreateEngineers();
        CreateTasks();
    }
}
