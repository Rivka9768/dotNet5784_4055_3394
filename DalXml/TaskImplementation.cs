

using DalApi;
using DO;
using System.Data.Common;
using System.Linq;
using System.Xml.Linq;

namespace Dal;

internal class TaskImplementation : ITask
{
    public int Create(DO.Task item)
    {
        XElement tasks = XMLTools.LoadListFromXMLElement("tasks");
        int id = Config.NextTaskId;
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
           (item.FinalDate != null ? new XElement("FinalDate", item.FinalDate) : null),
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
            Where(t => t.Element("Id")?.Value == id.ToString()).FirstOrDefault()
            ?? throw new DalDoesNotExistException($"Task with ID={id} doesn't exist");
        tempTask.Remove();
        XMLTools.SaveListToXMLElement(tasks, "tasks");
    }

    static DO.Task? getTask(XElement tempTask)
    {

        return tempTask.ToIntNullable("Id") is null ? null : new DO.Task()
        {
            Id = (int)tempTask?.Element("Id")!,
            Description = (string)tempTask?.Element("Description")!,
            ProductionDate = (DateTime)tempTask?.Element("ProductionDate")!,
            Deadline = (DateTime)tempTask?.Element("Deadline")!,
            Difficulty = (EngineerExperience)(int)tempTask?.Element("Difficulty")!,
            EngineerId = (int)tempTask?.Element("EngineerId")!,
            Milestone = (bool)tempTask?.Element("Milestone")!,
            StartDate = (DateTime)tempTask?.Element("StartDate")!,
            EstimatedEndDate = (DateTime)tempTask?.Element("EstimatedEndDate")!,
            FinalDate = (DateTime)tempTask?.Element("FinalDate")!,
            TaskNickname = (string)tempTask?.Element("TaskNickname")!,
            Remarks = (string)tempTask?.Element("Remarks")!,
            Products = (string)tempTask?.Element("Products")!,
        };
    }


    public DO.Task? Read(int id)
    {
        XElement tasks = XMLTools.LoadListFromXMLElement("tasks");
        DO.Task? t= tasks.Elements().Where(t => t.ToIntNullable("Id") == id).Select(t => getTask(t)).FirstOrDefault(); 
        return t; 
/*        return getTask(tasks.Elements("task").FirstOrDefault(t => t.Element("Id")?.Value == id.ToString()));*/
    }

    public DO.Task? Read(Func<DO.Task, bool> filter)
    {
        XElement tasks = XMLTools.LoadListFromXMLElement("tasks");
        return tasks.Elements().Select(s => getTask(s)).Where(filter!).FirstOrDefault();
    }

    public IEnumerable<DO.Task?> ReadAll(Func<DO.Task, bool>? filter = null)
    {
        return (filter is null) ? XMLTools.LoadListFromXMLElement("tasks").Elements().Select(s => getTask(s))
            : XMLTools.LoadListFromXMLElement("tasks").Elements().Select(s => getTask(s)).Where(filter!);
    }

    public void Update(DO.Task item)
    {
        Delete(item.Id);
        Create(item);
    }
}
