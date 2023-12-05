

using DalApi;
using DO;
namespace Dal;

internal class EngineerImplementation : IEngineer
{
    /// <summary>
    /// creates a new engineer
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int Create(Engineer item)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");
        Engineer engineer = (from e in engineers
                             let eId = e.Id
                             where eId == item.Id
                             select e).FirstOrDefault()!;
        if (engineer == null)
        {
            engineers.Add(item);
            XMLTools.SaveListToXMLSerializer<Engineer>(engineers, "engineers");
            return item.Id;
        }
            throw new DalAlreadyExistsException($"Engineer with ID={item.Id} already exists");
    }

    /// <summary>
    /// delete a engineer
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");
        Engineer engineer = (from e in engineers
                             let eId = e.Id
                             where eId == id
                             select e).FirstOrDefault()!;
        if (engineer == null)
            throw new DalDoesNotExistException($"Engineer with ID={id} does not exists");
        engineers.Remove(engineer);
        XMLTools.SaveListToXMLSerializer<Engineer>(engineers, "engineers");
    }
    /// <summary>
    /// find a engineer by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Engineer? Read(int id)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");
        Engineer engineer = (from e in engineers
                             let eId = e.Id
                             where eId == id
                             select e).FirstOrDefault()!;
        return engineer;
        throw new NotImplementedException();
    }

    /// <summary>
    /// finds a engineer by a boolian condition
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public Engineer? Read(Func<Engineer, bool> filter)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");
        return (from item in engineers
                where filter(item)
                select item).FirstOrDefault();
    }
    /// <summary>
    /// returns all engineers which answers to the boolian condition or if the function is called with no parameters than returns all engineers
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");
        if (filter != null)
        {
            return from item in engineers
                   where filter(item)
                   select item;
        }
        return from item in engineers
               select item;
    }

    /// <summary>
    /// updates a specific engineer
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Update(Engineer item)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");
        Engineer engineer = (from e in engineers
                             let eId = e.Id
                             where eId == item.Id
                             select e).FirstOrDefault()!;
        if (engineer == null)
            throw new DalDoesNotExistException($"Engineer with ID={item.Id} does not exists");
        engineers.Remove(engineer);
        engineers.Add(item);
        XMLTools.SaveListToXMLSerializer<Engineer>(engineers, "engineers");
    }
}
