using Dal;
using DalApi;
using DO;
using System.Reflection.Emit;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DalTest
{
    internal class Program
    {
        /*        private static readonly IDal s_dal = new Dal.DalList(); //stage 2*/
        /*        private static readonly IDal s_dal = new Dal.DalXml(); //stage 3*/
        private static readonly IDal s_dal = Factory.Get; //stage 4
        public enum Entities { EXIT, ENGINEER, TASK, DEPENDENCY };
        public enum Actions { EXIT, CREATE, READ, READALL, UPDATE, DELETE };
        
        /// <summary>
        /// creates a new engineer
        /// </summary>
        private static void EngineerCreate()
        {
            Console.WriteLine("Please enter id, name , level, salary per hour, email");
            int.TryParse(Console.ReadLine(), out int id);
            string name = Console.ReadLine()!;
            int.TryParse(Console.ReadLine(), out int level);
            double.TryParse(Console.ReadLine(), out double salary);
            string? email = Console.ReadLine();
            Engineer newEngineer = new(id, name, (EngineerExperience)level, salary, email);
            try
            {
                s_dal.Engineer!.Create(newEngineer);
            }
            catch (DalAlreadyExistsException e)
            {
                Console.WriteLine(e);
            }
        }
        
        /// <summary>
        /// shows to the user detailes of a specific enginer
        /// </summary>
        private static void EngineerRead()
        {
            Console.WriteLine("Please enter the id:");
            string id = Console.ReadLine()!;
            Console.WriteLine(s_dal.Engineer?.Read(Convert.ToInt32(id)));
        }
        
        /// <summary>
        /// shows to the user the details of all engineers
        /// </summary>
        private static void EngineerReadAll()
        {
            List<Engineer> engineers = s_dal.Engineer!.ReadAll().ToList()!;
            engineers.ForEach(engineer => { Console.WriteLine(engineer); });
        }
        
        /// <summary>
        /// updates custom details of a specific engineer
        /// </summary>
        private static void EngineerUpDate()
        {
            Console.WriteLine("Please enter the id:");
            int.TryParse(Console.ReadLine(), out int id);
            Engineer? engineer = s_dal.Engineer?.Read(id);
            Console.WriteLine(engineer);
            Console.WriteLine("Please enter the properties you want to update: name , level, salary per hour, email");
            string? name = Console.ReadLine();
            string? level = Console.ReadLine();
            string? salary = Console.ReadLine();
            string? email = Console.ReadLine();
            Engineer newEngineer = new(engineer.Id, (name != "") ? name : engineer.Name, (level !="") ? (EngineerExperience)(Convert.ToInt32(level)) : engineer.Level,
               (salary !="") ? Convert.ToDouble(salary) : engineer.SaleryPerHour, (email != "") ? email : engineer.Email);
            try
            {
                s_dal.Engineer!.Update(newEngineer);
            }
            catch (DalDoesNotExistException e)
            {
                Console.WriteLine(e);

            }
        }
        
        /// <summary>
        /// deletes a specific enginner
        /// </summary>
        private static void EngineerDelete()
        {
            Console.WriteLine("Please enter the id:");
            string id = Console.ReadLine()!;
            try
            {
                s_dal.Engineer!.Delete(Convert.ToInt32(id));
            }
            catch (DalDoesNotExistException e)
            {
                Console.WriteLine(e);
            }
        }
        
        /// <summary>
        /// the engineer navigation menue
        /// </summary>
        private static void EngineerFunc()
        {
            int action;
            do
            {
                Console.WriteLine("Enter your choise:\n" + "1 - for creating an Enginner\n" + "2 - for viewing an Enginner\n"
                    + "3 - for viewing all Enginners\n" + "4 - for updating an Enginner\n" + "5 - for deleting an Enginner\n" + "0 - for exiting to the previous menue\n");
                string? input = Console.ReadLine();
                action = Convert.ToInt32(input);
                switch ((Actions)action)
                {
                    case Actions.EXIT:
                        return;
                    case Actions.CREATE:
                        EngineerCreate();
                        break;
                    case Actions.READ:
                        EngineerRead();
                        break;
                    case Actions.READALL:
                        EngineerReadAll();
                        break;
                    case Actions.UPDATE:
                        EngineerUpDate();
                        break;
                    case Actions.DELETE:
                        EngineerDelete();
                        break;
                    default:
                        return;

                }
            }
            while (action != 0);
        }


        /// <summary>
        /// creates a new task
        /// </summary>
        private static void TaskCreate()
        {
            Console.WriteLine("Please enter  description , production date, deadline, task dificulty, engineer id," +
                " milestone, start date ,estimated end date, final date,task nickname, remarks,products:");
            string description = Console.ReadLine()!;
            DateTime.TryParse(Console.ReadLine(), out DateTime productionDate);
            DateTime.TryParse(Console.ReadLine(), out DateTime deadline);
            int.TryParse(Console.ReadLine(), out int difficulty);
            int.TryParse(Console.ReadLine(), out int engineerId);
            bool.TryParse(Console.ReadLine(), out bool milestone);
            DateTime.TryParse(Console.ReadLine(), out DateTime startDate);
            DateTime.TryParse(Console.ReadLine(), out DateTime estimatedEndDate);
            DateTime.TryParse(Console.ReadLine(), out DateTime finalDate);
            string? taskNickname = Console.ReadLine();
            string? remarks = Console.ReadLine();
            string? products = Console.ReadLine();
            DO.Task newTask = new(0, description, productionDate, deadline, (EngineerExperience)difficulty,
                engineerId, milestone, startDate, estimatedEndDate, finalDate, taskNickname, remarks, products);
            s_dal.Task!.Create(newTask);
        }

        /// <summary>
        /// shows to the user detailes of a specific task
        /// </summary>
        private static void TaskRead()
        {
            Console.WriteLine("Please enter the id:");
            string id = Console.ReadLine()!;
            Console.WriteLine(s_dal.Task?.Read(Convert.ToInt32(id)));
        }
        
        /// <summary>
        /// shows to the user the details of all tasks
        /// </summary>
        private static void TaskReadAll()
        {
            List<DO.Task> tasks = s_dal.Task!.ReadAll().ToList()!;
            tasks.ForEach(task => { Console.WriteLine(task) ; });
        }
        
        /// <summary>
        /// updates custom details of a specific task
        /// </summary>
        private static void TaskUpDate()
        {
            Console.WriteLine("Please enter the id:");
            string id = Console.ReadLine()!;
            DO.Task? task = s_dal.Task?.Read(Convert.ToInt32(id));
            Console.WriteLine(task);
            Console.WriteLine("Please enter the properties you want to update:  description , production date, deadline, task dificulty, " +
                "engineer id, milestone, start date ,estimated end date, final date,task nickname, remarks,products");
            string? description = Console.ReadLine();
            DateTime.TryParse(Console.ReadLine(), out DateTime productionDate);
            DateTime.TryParse(Console.ReadLine(), out DateTime deadline);
            string? difficulty = Console.ReadLine();
            string? engineerId = Console.ReadLine();
            string? milestone = Console.ReadLine();
            DateTime.TryParse(Console.ReadLine(), out DateTime startDate);
            DateTime.TryParse(Console.ReadLine(), out DateTime estimatedEndDate);
            DateTime.TryParse(Console.ReadLine(), out DateTime finalDate);
            string? taskNickname = Console.ReadLine();
            string? remarks = Console.ReadLine();
            string? products = Console.ReadLine();
            DO.Task newTask = new(task.Id, (description != "") ? description : task.Description, (productionDate != DateTime.MinValue) ?productionDate : task.ProductionDate, (deadline != DateTime.MinValue) ? deadline : task.Deadline,
                (difficulty!="") ? (EngineerExperience)(Convert.ToInt32(difficulty)) : task.Difficulty, (engineerId != "") ? Convert.ToInt32(engineerId) : task.EngineerId, (milestone != "") ? Convert.ToBoolean(milestone) : task.Milestone
                , (startDate != DateTime.MinValue) ? startDate : task.StartDate, (estimatedEndDate != DateTime.MinValue) ? estimatedEndDate : task.EstimatedEndDate, (finalDate != DateTime.MinValue) ? finalDate : task.FinalDate
                , (taskNickname != "") ? taskNickname : task.TaskNickname, (remarks != "") ? remarks : task.Remarks, (products != "") ? products : task.Products);
            try
            {
                s_dal.Task!.Update(newTask);
            }
            catch (DalDoesNotExistException e)
            {
                Console.WriteLine(e);

            }
        }
        
        /// <summary>
        /// deletes a specific task
        /// </summary>
        private static void TaskDelete() 
        {
            Console.WriteLine("Please enter the id:");
            string id = Console.ReadLine()!;
            try
            {
                s_dal.Task!.Delete(Convert.ToInt32(id));
            }
            catch (DalDoesNotExistException e)
            {
                Console.WriteLine(e);
            }
        }
        
        /// <summary>
        /// the task navigation menue
        /// </summary>
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
                        TaskCreate();
                        break;
                    case Actions.READ:
                        TaskRead();
                        break;
                    case Actions.READALL:
                        TaskReadAll();
                        break;
                    case Actions.UPDATE:
                        TaskUpDate();
                        break;
                    case Actions.DELETE:
                        TaskDelete();
                        break;
                    default:
                        return;

                }
            }
            while (action != 0);
        }

        /// <summary>
        /// creates a new dependency
        /// </summary>
        private static void DependencyCreate()
        {
            Console.WriteLine("Please enter  id of previous task , id of dependant task");
            int.TryParse(Console.ReadLine(), out int idPreviousTask);
            int.TryParse(Console.ReadLine(), out int idDependantTask);
            Dependency newDependency = new(0, idPreviousTask, idDependantTask);
            s_dal.Dependency!.Create(newDependency);
        }

        /// <summary>
        /// shows to the user detailes of a specific dependency
        /// </summary>
        private static void DependencyRead()
        {

            Console.WriteLine("Please enter the id:");
            string id = Console.ReadLine()!;
            Console.WriteLine(s_dal.Dependency?.Read(Convert.ToInt32(id)));

        }
        
        /// <summary>
        /// shows to the user the details of all dependencies
        /// </summary>
        private static void DependencyReadAll()
        {

            List<Dependency> dependencies = s_dal.Dependency!.ReadAll().ToList()!;
            dependencies.ForEach(dependency => { Console.WriteLine(dependency); });

        }
        
        /// <summary>
        /// updates custom details of a specific dependency
        /// </summary>
        private static void DependencyUpDate()
        {
            Console.WriteLine("Please enter the id:");
            int.TryParse(Console.ReadLine(), out int id);
            Dependency? dependency = s_dal.Dependency?.Read(id);
            Console.WriteLine(dependency);
            Console.WriteLine("Please enter the properties you want to update:  id previous task , id dependant task");
            string? idPreviousTask = Console.ReadLine();
            string? idDependantTask= Console.ReadLine();
            Dependency newDependency = new(dependency.Id, (idPreviousTask != "") ? Convert.ToInt32(idPreviousTask) : dependency.IdPreviousTask,
                (idDependantTask != "") ? Convert.ToInt32(idDependantTask) : dependency.IdDependantTask);
            try
            {
                s_dal.Dependency!.Update(newDependency);
            }
            catch (DalDoesNotExistException e)
            {
                Console.WriteLine(e);

            }
        }
        
        /// <summary>
        /// deletes a specific dependency
        /// </summary>
        private static void DependencyDelete()
        {
            Console.WriteLine("Please enter the id:");
            string id = Console.ReadLine()!;
            try
            {
                s_dal.Dependency!.Delete(Convert.ToInt32(id));
            }
            catch (DalDoesNotExistException e)
            {
                Console.WriteLine(e);
            }
        }
        
        /// <summary>
        /// the dependency navigation menue
        /// </summary>
        private static void dependencyFunc()
        {
            int action;
            do
            {
                Console.WriteLine("Enter your choise:\n" + "1 - for creating a Dependency\n" + "2 - for viewing a Dependency\n"
                    + "3 - for viewing all Dependencies\n" + "4 - for updating a Dependency\n" + "5 - for deleting a Dependency\n" + "0 - for exiting to the previous menue\n");
                string? input = Console.ReadLine();
                action = Convert.ToInt32(input);
                switch ((Actions)action)
                {
                    case Actions.EXIT:
                        return;
                    case Actions.CREATE:
                        DependencyCreate();
                        break;
                    case Actions.READ:
                        DependencyRead();
                        break;
                    case Actions.READALL:
                        DependencyReadAll();
                        break;
                    case Actions.UPDATE:
                        DependencyUpDate();
                        break;
                    case Actions.DELETE:
                        DependencyDelete();
                        break;
                    default:
                        return;

                }
            }
            while (action != 0);
        }
        
        /// <summary>
        /// main navigation menue
        /// </summary>
        private static void MainMenue()
        {
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
                        EngineerFunc();
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
        static void Main()
        {
            try
            {
                Initialization.Do(s_dal);
            }catch(NullReferenceException e) 
            { 
                Console.WriteLine(e);    
            }

            MainMenue();
            return;
        }

    }
}