using Dal;
using DalApi;
using DO;
using System.Reflection.Emit;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DalTest
{
    internal class Program
    {
        private static IEngineer? s_dalEngineer = new EngineerImplementation(); //stage 1
        private static ITask? s_dalTask = new TaskImplementation(); //stage 1
        private static IDependency? s_dalDependency = new DependencyImplementation(); //stage 1
        public enum Entities { EXIT, ENGINEER, TASK, DEPENDENCY };
        public enum Actions { EXIT, CREATE, READ, READALL, UPDATE, DELETE };

        /// איך להדפיס את האוביקטים של הישויות בתור אוביקטים?????
        private static void engineerCreate()
        {
            Console.WriteLine("Please enter id, name , level, salary per hour, email");
            string id = Console.ReadLine()!;
            string name = Console.ReadLine()!;
            string level = Console.ReadLine()!;
            string salary = Console.ReadLine()!;
            string? email = Console.ReadLine();
            Engineer newEngineer = new(Convert.ToInt32(id), name, (EngineerExperience)Convert.ToInt32(level), Convert.ToDouble(salary), email);
            try
            {
                s_dalEngineer!.Create(newEngineer);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        private static void engineerReadAll()
        {
            List<Engineer> engineers = s_dalEngineer!.ReadAll();
            engineers.ForEach(engineer => { Console.WriteLine(engineer); });
        }
        private static void engineerUpDate()
        {
            Console.WriteLine("Please enter the id:");
            string id = Console.ReadLine()!;
            ///האם צריך לומר אם אין כזה מהנדס???????????
            Engineer? engineer = s_dalEngineer?.Read(Convert.ToInt32(id));
            Console.WriteLine(engineer);
            Console.WriteLine("Please enter the properties you want to update: name , level, salary per hour, email");
            string? name = Console.ReadLine();
            string? level = Console.ReadLine();
            string? salary = Console.ReadLine();
            string? email = Console.ReadLine();
            ///???מה לעשות אם אין כזה אוביקט אם המספר זהות הזה
            Engineer newEngineer = new(engineer.Id, (name != null) ? name : engineer.Name, (level != null) ? (EngineerExperience)Convert.ToInt32(level) : engineer.Level,
               (salary != null) ? Convert.ToDouble(salary) : engineer.SaleryPerHour, (email != null) ? email : engineer.Email);
            try
            {
                s_dalEngineer!.Update(newEngineer);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
        }
        private static void engineerDelete()
        {
            Console.WriteLine("Please enter the id:");
            string id = Console.ReadLine()!;
            try
            {
                s_dalEngineer!.Delete(Convert.ToInt32(id));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        private static void engineerRead()
        {
            Console.WriteLine("Please enter the id:");
            string id = Console.ReadLine()!;
            Console.WriteLine(s_dalEngineer?.Read(Convert.ToInt32(id)));
        }
        private static void engineerFunc()
        {
            int action;
            do
            {
                Console.WriteLine("Enter your choise:\n" + "1 - for creating an Enginner\n" + "2 - for viewing an Enginner\n" 
                    + "3 - for viewing all Enginners\n" + "4 - for updating an Enginner\n" + "5 - for deleting an Enginner\n"+ "0 - for exiting to the previous menue\n");
                string? input = Console.ReadLine();
                action = Convert.ToInt32(input);
                switch ((Actions)action)
                {
                    case Actions.EXIT:
                        return;
                    case Actions.CREATE:
                       engineerCreate();
                       break;
                    case Actions.READ:
                        engineerRead();
                            break;
                    case Actions.READALL:
                        engineerReadAll();
                            break;                       
                    case Actions.UPDATE:
                            engineerUpDate();                          
                            break;
                    case Actions.DELETE:
                        engineerDelete();
                        break;
                    default:
                        return;

                }
            }
            while (action != 0);
        }
        private static void taskCreate()
        {
            Console.WriteLine("Please enter id, description , production date, deadline, task dificulty, engineer id," +
                " milestone, start date ,estimated end date, final date,task nickname, remarks,products:");
            string id = Console.ReadLine()!;
            string description = Console.ReadLine()!;
            string productionDate = Console.ReadLine()!;
            string deadline = Console.ReadLine()!;
            string difficulty = Console.ReadLine()!;
            string? engineerId = Console.ReadLine();
            string? milestone = Console.ReadLine();
            string? startDate = Console.ReadLine();
            string? estimatedEndDate = Console.ReadLine();
            string? finalDate = Console.ReadLine();
            string? taskNickname = Console.ReadLine();
            string? remarks = Console.ReadLine();
            string? products = Console.ReadLine();
            DO.Task newTask = new(Convert.ToInt32(id), description, DateTime.Parse(productionDate),DateTime.Parse(deadline), (EngineerExperience)Convert.ToInt32(difficulty),
                Convert.ToInt32(engineerId), Convert.ToBoolean(milestone), DateTime.Parse(startDate), DateTime.Parse(estimatedEndDate), DateTime.Parse(finalDate), taskNickname, remarks, products);
            try
            {
                s_dalTask!.Create(newTask);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        private static void taskRead() 
        {
            Console.WriteLine("Please enter the id:");
            string id = Console.ReadLine()!;
            Console.WriteLine(s_dalTask?.Read(Convert.ToInt32(id)));
        }
        private static void taskReadAll() 
        {
            List<DO.Task> tasks = s_dalTask!.ReadAll();
            tasks.ForEach(task => { Console.WriteLine(task); });
        }
        private static void taskUpDate() 
        {
            Console.WriteLine("Please enter the id:");
            string id = Console.ReadLine()!;
            ///האם צריך לומר אם אין כזה משימה???????????
            DO.Task? task = s_dalTask?.Read(Convert.ToInt32(id));
            Console.WriteLine(task);
            Console.WriteLine("Please enter the properties you want to update:  description , production date, deadline, task dificulty, " +
                "engineer id, milestone, start date ,estimated end date, final date,task nickname, remarks,products");
            string? description = Console.ReadLine();
            string? productionDate = Console.ReadLine();
            string? deadline = Console.ReadLine();
            string? difficulty = Console.ReadLine();
            string? engineerId = Console.ReadLine();
            string? milestone = Console.ReadLine();
            string? startDate = Console.ReadLine();
            string? estimatedEndDate = Console.ReadLine();
            string? finalDate = Console.ReadLine();
            string? taskNickname = Console.ReadLine();
            string? remarks = Console.ReadLine();
            string? products = Console.ReadLine();
            ///???מה לעשות אם אין כזה אוביקט אם המספר זהות הזה
            DO.Task newTask = new(task.Id, (description != null) ? description : task.Description, (productionDate != null) ? DateTime.Parse(productionDate) : task.ProductionDate, (deadline != null) ? DateTime.Parse(deadline) : task.Deadline,
                (difficulty != null) ? (EngineerExperience)Convert.ToInt32(difficulty) : task.Difficulty,(engineerId != null) ? Convert.ToInt32(engineerId) : task.EngineerId, (milestone != null) ? Convert.ToBoolean(milestone) : task.Milestone
                ,(startDate != null) ? DateTime.Parse(startDate) : task.StartDate, (estimatedEndDate != null) ? DateTime.Parse(estimatedEndDate) : task.EstimatedEndDate, (finalDate != null) ? DateTime.Parse(finalDate) : task.FinalDate
                , (taskNickname != null) ? taskNickname : task.TaskNickname, (remarks != null) ? remarks : task.Remarks, (products != null) ? products : task.Products);
            try
            {
                s_dalTask!.Update(newTask);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
        }
        private static void taskDelete() { }
        private static void taskFunc() 
        {
            int action;
            do
            {
                Console.WriteLine("Enter your choise:\n" + "1 - for creating a Task\n" + "2 - for viewing a Task\n"
                    + "3 - for viewing all Tasks\n" + "4 - for updating a Task\n" + "5 - for deleting a Task\n" + "0 - for exiting to the previous menue\n");
                string? input = Console.ReadLine();
                action = Convert.ToInt32(input);
                switch ((Actions)action)
                {
                    case Actions.EXIT:
                        return;
                    case Actions.CREATE:
                        taskCreate();
                        break;
                    case Actions.READ:
                        taskRead();
                        break;
                    case Actions.READALL:
                        taskReadAll();
                        break;
                    case Actions.UPDATE:
                        taskUpDate();
                        break;
                    case Actions.DELETE:
                        taskDelete();
                        break;
                    default:
                        return;

                }
            }
            while (action != 0);
        }

        private static void dependencyFunc() 
        { 
        
        }
        static void Main()
        {

            Initialization.Do(s_dalTask, s_dalDependency, s_dalEngineer);
            int entity;
            do
            {
                Console.WriteLine("Enter your choise:\n" + "1 - for Enginner\n" + "2 - for Task\n" + "3 - for Dependency\n" + "0 - for exiting the program\n");
                string? input = Console.ReadLine();
                entity = Convert.ToInt32(input);
                switch ((Entities)entity)
                {
                    case Entities.EXIT:
                        return;
                    case Entities.ENGINEER:
                        engineerFunc();
                         break;
                    case Entities.TASK:
                        taskFunc();
                        break;
                    case Entities.DEPENDENCY:
                        dependencyFunc();
                        break;
                    default:
                        return;
                }
            }
            while (entity != 0);
        }
    }
}