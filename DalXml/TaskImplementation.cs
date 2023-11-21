

using DalApi;
using DO;
using System.Xml.Linq;

namespace Dal;

internal class TaskImplementation : ITask
{
    public int Create(DO.Task item)
    {
        XElement tasks = XMLTools.LoadListFromXMLElement("tasks");
        int id = XMLTools.GetAndIncreaseNextId("Config", "NextTaskId");
        XElement myTask = new("task",
            new XElement("Id", id),
            new XElement("Description", item.Description),
            new XElement("ProductionDate", item.ProductionDate),
            new XElement("Deadline", item.Deadline),
            new XElement("Difficulty", item.Difficulty),
           (item.EngineerId != null ? new XElement("EngineerId", item.EngineerId) : null),
             (item.Milestone != null? new XElement("Milestone", item.Milestone) : new XElement("Milestone", false)),
           (item.StartDate != null ? new XElement("StartDate", item.StartDate) : null),
           (item.EstimatedEndDate != null ? new XElement("EstimatedEndDate", item.EstimatedEndDate) : null),
           (item.TaskNickname != "" ? new XElement("TaskNickname", item.TaskNickname) : null),
           (item.Remarks != "" ? new XElement("Remarks", item.Remarks) : null),
           (item.Products != "" ? new XElement("Products", item.Products) : null));
        tasks.Add(myTask);
        XMLTools.SaveListToXMLElement(tasks, "tasks");
        return id;
    }
    public void Delete(int id)
    {
        XElement tasks = XMLTools.LoadListFromXMLElement("tasks");
        XElement? tempTask = tasks!.Elements("task").
            Where(p => p.Element("Id")?.Value == id.ToString()).FirstOrDefault()
            ?? throw new DalDoesNotExistException($"Dependency with ID={id} doesn't exist");
        tempTask.Remove();
        XMLTools.SaveListToXMLElement(tasks, "tasks");
    }




    public DO.Task? Read(int id)
    {

    }

    public DO.Task? Read(Func<DO.Task, bool> filter)
    {
       
    }

    public IEnumerable<DO.Task?> ReadAll(Func<DO.Task, bool>? filter = null)
    {
        
    }

    public void Update(DO.Task item)
    {
        
    }
}
