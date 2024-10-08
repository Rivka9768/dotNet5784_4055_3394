﻿

using DalApi;
using DO;
using System.Data.Common;
using System.Linq;
using System.Xml.Linq;

namespace Dal;

internal class TaskImplementation : ITask
{
    /// <summary>
    /// creates a new task
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int Create(DO.Task item)
    {
        XElement tasks = XMLTools.LoadListFromXMLElement("tasks");
        int id = ((item.Id > 0) ? item.Id : Config.NextTaskId);
        XElement myTask = new("task",
            new XElement("Id", id),
            new XElement("Description", item.Description),
            new XElement("ProductionDate", item.ProductionDate),
            new XElement("Deadline", item.Deadline),
            new XElement("Difficulty", item.Difficulty),
           (item.EngineerId != null ? new XElement("EngineerId", item.EngineerId) : null),
             (item.Milestone != null ? new XElement("Milestone", item.Milestone) : new XElement("Milestone", false)),
             (item.Duration != null ? new XElement("Duration", item.Duration) : null),
             (item.EstimatedStartDate != null ? new XElement("EstimatedStartDate", item.EstimatedStartDate) : null),
           (item.StartDate != null ? new XElement("StartDate", item.StartDate) : null),
           (item.EstimatedEndDate != null ? new XElement("EstimatedEndDate", item.EstimatedEndDate) : null),
           (item.FinalDate != null ? new XElement("FinalDate", item.FinalDate) : null),
           (item.TaskNickname != "" ? new XElement("TaskNickname", item.TaskNickname) : null),
           (item.Remarks != "" ? new XElement("Remarks", item.Remarks) : null),
           (item.Products != "" ? new XElement("Products", item.Products) : null));
        tasks.Add(myTask);
        XMLTools.SaveListToXMLElement(tasks, "tasks");
        return id;
    }

    /// <summary>
    /// delete a task
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id)
    {
        XElement tasks = XMLTools.LoadListFromXMLElement("tasks");
        XElement? tempTask = tasks!.Elements("task").
            Where(t => t.Element("Id")?.Value == id.ToString()).FirstOrDefault()
            ?? throw new DalDoesNotExistException($"Task with ID={id} doesn't exist");
        tempTask.Remove();
        XMLTools.SaveListToXMLElement(tasks, "tasks");
    }

    /// <summary>
    /// this function is inorder to help me get the values which is in the task xml
    /// </summary>
    /// <param name="tempTask"></param>
    /// <returns></returns>
    private DO.Task? GetTask(XElement tempTask)
    {
        if (tempTask.ToIntNullable("Id") is null)
            return null;
        EngineerExperience tempDifficulty;
        Enum.TryParse(tempTask?.Element("Difficulty").Value!, out tempDifficulty);
        return new DO.Task()
        {
            Id = (int)tempTask?.Element("Id")!,
            Description = (string)tempTask?.Element("Description")!,
            ProductionDate = (DateTime)tempTask?.Element("ProductionDate")!,
            Deadline = (DateTime)tempTask?.Element("Deadline")!,
            Difficulty = tempDifficulty,
            EngineerId = (int)tempTask?.Element("EngineerId")!,
            Milestone = (bool)tempTask?.Element("Milestone")!,
            Duration = (TimeSpan)tempTask?.Element("Duration")!,
            EstimatedStartDate = (DateTime)tempTask?.Element("EstimatedStartDate")!,
            StartDate = ((tempTask?.Element("StartDate")) != null) ? ((DateTime)tempTask?.Element("StartDate")!) : (new DateTime()),
            EstimatedEndDate = ((tempTask?.Element("EstimatedEndDate")) != null) ? ((DateTime)tempTask?.Element("EstimatedEndDate")!) : (new DateTime()),
            FinalDate = ((tempTask?.Element("FinalDate")) != null) ? ((DateTime)tempTask?.Element("FinalDate")!) : (new DateTime()),
            TaskNickname = ((tempTask?.Element("TaskNickname")) != null) ? ((string)tempTask?.Element("TaskNickname")!) : "",
            Remarks = ((tempTask?.Element("Remarks")) != null) ? ((string)tempTask?.Element("Remarks")!) : "",
            Products = ((tempTask?.Element("Products")) != null) ? ((string)tempTask?.Element("Products")!) : "",
        };
    }

    /// <summary>
    /// find a task by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public DO.Task? Read(int id)
    {
        XElement tasks = XMLTools.LoadListFromXMLElement("tasks");
        DO.Task? t = tasks.Elements().Where(t => t.ToIntNullable("Id") == id).Select(t => GetTask(t)).FirstOrDefault();
        return t;
    }

    /// <summary>
    /// finds a task by a boolian condition
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public DO.Task? Read(Func<DO.Task, bool> filter)
    {
        XElement tasks = XMLTools.LoadListFromXMLElement("tasks");
        return tasks.Elements().Select(s => GetTask(s)).Where(filter!).FirstOrDefault();
    }
    /// <summary>
    /// returns all tasks which answers to the boolian condition or if the function is called with no parameters than returns all tasks
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<DO.Task?> ReadAll(Func<DO.Task, bool>? filter = null)
    {
        return (filter is null) ? XMLTools.LoadListFromXMLElement("tasks").Elements().Select(s => GetTask(s))
            : XMLTools.LoadListFromXMLElement("tasks").Elements().Select(s => GetTask(s)).Where(filter!);
    }

    /// <summary>
    /// updates a specific task
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Update(DO.Task item)
    {
        Delete(item.Id);
        Create(item);
    }
}
