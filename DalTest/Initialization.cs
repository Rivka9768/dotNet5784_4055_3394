

namespace DalTest;
using DalApi;
using DO;
using static System.Runtime.InteropServices.JavaScript.JSType;

public static class Initialization
{

    private static IDependency? s_dalDependency; //stage 1
    private static IEngineer? s_dalEngineer; //stage 1
    private static ITask? s_dalTask; //stage 1

    private static readonly Random s_rand = new();

    private static void createEngineers()
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

        foreach (string _name in engineerNames)
        {
            int i = 1;
            int _id;
            do
                _id = s_rand.Next(100000000, 999999999);
            while (s_dalEngineer!.Read(_id) != null);
            string _email = engineerEmails[i];
            EngineerExperience _level = (EngineerExperience)s_rand.Next(0, 5);
            double _saleryPerHour = 100 * ((int)_level+1);
            Engineer newEngineer = new(_id, _name, _email, _level, _saleryPerHour);
            s_dalEngineer!.Create(newEngineer);
        }
    }
    private static void createDependencys()
    {
        int _id = 0;
        int _idPreviousTask;
        int _idDependantTask;
        for (int i = 0; i < 40; i++) 
        {
            List<Dependency> dependencies = s_dalDependency!.ReadAll();
            do
            {
                _idPreviousTask = s_rand.Next(1, 16);
                _idDependantTask = s_rand.Next(_idPreviousTask + 1, 17);
            } while (dependencies.Find((d) =>
                (d.IdPreviousTask == _idPreviousTask &&d.IdDependantTask== _idDependantTask)) != null);
            Dependency newDependency = new(_id, _idPreviousTask, _idDependantTask);
            s_dalDependency!.Create(newDependency);
        }
    }
    private static void createTasks()
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
            List<Engineer> engineers= s_dalEngineer!.ReadAll();
            EngineerExperience _difficulty = (EngineerExperience)s_rand.Next(0, 5);
            List<Engineer> qualifiedEngineers = engineers.FindAll((e) => e.Level >= _difficulty);
            int amountEngineers = qualifiedEngineers.Count();
            int randomEngineerNum = s_rand.Next(0, amountEngineers+1);
            int _engineerId = qualifiedEngineers[randomEngineerNum].Id;
            Task newTask = new(_id, _description, _productionDate, _deadline, _difficulty, _engineerId, _milestone);
            s_dalTask!.Create(newTask);
        }
    }

    public static IEngineer? dalEngineer;
    public static ITask? dalETask;
    public static IDependency? dalDependency;
    public static void Do(ITask s_dalTask, IDependency s_dalDependency, IEngineer s_dalEngineer)
    {
        s_dalTask = dalETask ?? throw new NullReferenceException("DAL can not be null!");
        s_dalDependency = dalDependency ?? throw new NullReferenceException("DAL can not be null!");
        s_dalEngineer = dalEngineer ?? throw new NullReferenceException("DAL can not be null!");
        createDependencys();
        createEngineers();
        createTasks();
    }
}
