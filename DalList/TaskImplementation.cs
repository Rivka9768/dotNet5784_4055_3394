

namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class TaskImplementation : ITask
{
    /// <summary>
    /// creates a new task
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int Create(Task item)
    {
        int id = DataSource.Config.NextTaskId;
        DataSource.Tasks.Add(item:item with { Id = id });
        return id;
    }

    /// <summary>
    /// delete a task
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id)
    {
        Task task = (from t in DataSource.Tasks
                        let tId = t.Id
                        where tId == id
                        select t).FirstOrDefault()!;
        if (task == null)
            throw new DalDoesNotExistException($"Task with ID={id} does not exists");
        DataSource.Tasks.Remove(task);
    }
    /// <summary>
    /// find a task by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task? Read(int id)
    {
        Task task = (from t in DataSource.Tasks
                    let tId = t.Id
                    where tId == id
                    select t).FirstOrDefault()!;
        return task;
    }


    /// <summary>
    /// updates a specific task
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
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

    /// <summary>
    /// finds a task by a boolian condition
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public Task? Read(Func<Task, bool> filter)
    {
            return (from item in DataSource.Tasks
                    where filter(item)
                    select item).FirstOrDefault();
    }
    /// <summary>
    /// returns all tasks which answers to the boolian condition or if the function is called with no parameters than returns all tasks
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
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
