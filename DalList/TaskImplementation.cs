

namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class TaskImplementation : ITask
{
    public int Create(Task item)
    {
        int id = DataSource.Config.NextTaskId;
        DataSource.Tasks.Add(item:item with { Id = id });
        return id;
    }

    public void Delete(int id)
    {
        Task? task = DataSource.Tasks.Find((t) => t.Id == id);
        if (task == null)
            throw new Exception("An object of type Task with such an ID does not exist");
        DataSource.Tasks.Remove(task);
    }

    public Task? Read(int id)
    {
        Task? task = DataSource.Tasks.Find((t) => t.Id == id);
        return task;
    }

    public List<Task> ReadAll()
    {
        return new List<Task>(DataSource.Tasks);
    }

    public void Update(Task item)
    {
        Task? task = DataSource.Tasks.Find((t) => t.Id == item.Id);
        if(task == null) 
            throw new Exception("An object of type Task with such an ID does not exist");
        DataSource.Tasks.Remove(task);
        DataSource.Tasks.Add(item);
    }
}
