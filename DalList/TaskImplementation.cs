

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
            throw new DalDeletionImpossible($"Can't delete Task with ID={id}");
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


    public void Update(Task item)
    {
        Task task = (from t in DataSource.Tasks
                    let tId = t.Id
                    where tId == item.Id
                    select t).FirstOrDefault()!;
        if (task == null) 
            throw new DalDoesNotExistException($"Task with ID={item.Id} does not exists");
        DataSource.Tasks.Remove(task);
        DataSource.Tasks.Add(item);
    }

    public Task? Read(Func<Task, bool> filter)
    {
            return (from item in DataSource.Tasks
                    where filter(item)
                    select item).FirstOrDefault();
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
