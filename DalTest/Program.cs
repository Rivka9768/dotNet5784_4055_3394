﻿using Dal;
using DalApi;
using DO;
using System.Reflection.Emit;

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
                    + "3 - for viewing all Enginners\n" + "4 - for updating an Enginner\n" + "5 - for deleting an Enginner\n" + "0 - for exiting to the previous menue\n");
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
        private static void taskCreate() { }
        private static void taskRead() { }
        private static void taskReadAll() { }
        private static void taskUpDate() { }
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

        private static void dependencyCreate() {
            Console.WriteLine("Please enter id, id previous task , id dependant task");          
            string id = Console.ReadLine()!;
            string idPreviousTask = Console.ReadLine()!;
            string idDependantTask = Console.ReadLine()!;

            Dependency newDependency = new(Convert.ToInt32(id) , Convert.ToInt32(idPreviousTask), Convert.ToInt32(idDependantTask));
            try
            {
                s_dalDependency!.Create(newDependency);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        private static void dependencyRead() {

            Console.WriteLine("Please enter the id:");
            string id = Console.ReadLine()!;
            Console.WriteLine(s_dalDependency?.Read(Convert.ToInt32(id)));

        }
        private static void dependencyReadAll() {

            List<Dependency> dependencies = s_dalDependency!.ReadAll();
            dependencies.ForEach(dependency => { Console.WriteLine(dependency); });

        }
        private static void dependencyUpDate()
        {
            Console.WriteLine("Please enter the id:");
            string id = Console.ReadLine()!;
            ///האם צריך לומר אם אין כזה תלות???????????
            Dependency? dependency = s_dalDependency?.Read(Convert.ToInt32(id));
            Console.WriteLine(dependency);
            Console.WriteLine("Please enter the properties you want to update:  id previous task , id dependant task");
            string? idPreviousTask = Console.ReadLine()!;
            string? idDependantTask = Console.ReadLine()!;
            ///???מה לעשות אם אין כזה אוביקט אם המספר זהות הזה
            Dependency newDependency = new(Convert.ToInt32(dependency.Id), (idPreviousTask != null) ? Convert.ToInt32(idPreviousTask) : dependency.IdPreviousTask,
                (idDependantTask != null) ? Convert.ToInt32(idDependantTask) : dependency.IdDependantTask);
            try
            {
                s_dalDependency!.Update(newDependency);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
        }
        private static void dependencyDelete() {
            Console.WriteLine("Please enter the id:");
            string id = Console.ReadLine()!;
            try
            {
                s_dalDependency!.Delete(Convert.ToInt32(id));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
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
                        dependencyCreate();
                        break;
                    case Actions.READ:
                        dependencyRead();
                        break;
                    case Actions.READALL:
                        dependencyReadAll();
                        break;
                    case Actions.UPDATE:
                        dependencyUpDate();
                        break;
                    case Actions.DELETE:
                        dependencyDelete();
                        break;
                    default:
                        return;

                }
            }
            while (action != 0);
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