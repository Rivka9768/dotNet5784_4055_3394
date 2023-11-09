

namespace DalTest;
using DalApi;
using DO;
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

        foreach (string _name in engineerNames ) 
        {
            int i = 1;
            int _id;
            do
                _id = s_rand.Next(100000000, 999999999);
            while (s_dalEngineer!.Read(_id) != null);
            string  _email = engineerEmails[i];
            EngineerExperience _level = s_rand.Next(EngineerExperience.Novice, EngineerExperience.Expert);
            double _saleryPerHour = 100 * (int)_level;
            Engineer newEngineer = new(_id,_name, _email, _level, _saleryPerHour);
            s_dalEngineer!.Create(newEngineer);
        }

    private static void createDependencys()
    {

    }
    private static void createTasks()
    {

    }
}
