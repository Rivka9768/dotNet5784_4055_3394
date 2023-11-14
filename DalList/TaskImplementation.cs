

namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class TaskImplementation : ITask
{
    public int Create(Task item)
    {
        int id = DataSource.Config.NextTaskId;
        DataSource.Tasks.Add(item:item with { Id = id });
        return id;
    }

    public void Delete(int id)
    {
        Task task = (from t in DataSource.Tasks
                        let tId = t.Id
                        where tId == id
                        select t).FirstOrDefault()!;
        if (task == null)
            throw new Exception("An object of type Task with such an ID does not exist");
        DataSource.Tasks.Remove(task);
    }

    public Task? Read(int id)
    {
        Task task = (from t in DataSource.Tasks
                    let tId = t.Id
                    where tId == id
                    select t).FirstOrDefault()!;
        return task;
    }

/*    public List<Task> ReadAll()
    {
        return new List<Task>(DataSource.Tasks);
    }*/

    public void Update(Task item)
    {
        Task task = (from t in DataSource.Tasks
                    let tId = t.Id
                    where tId == item.Id
                    select t).FirstOrDefault()!;
        if (task == null) 
            throw new Exception("An object of type Task with such an ID does not exist");
        DataSource.Tasks.Remove(task);
        DataSource.Tasks.Add(item);
    }
    public IEnumerable<Task> ReadAll(Func<Task, bool>? filter = null)
    {
        if (filter != null)
        {
            return from item in DataSource.Tasks
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Tasks
               select item;
    }
}
