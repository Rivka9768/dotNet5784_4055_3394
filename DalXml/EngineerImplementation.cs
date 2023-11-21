

using DalApi;
using DO;
namespace Dal;

internal class EngineerImplementation : IEngineer
{
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

    public Engineer? Read(int id)
    {
        throw new NotImplementedException();
    }

    public Engineer? Read(Func<Engineer, bool> filter)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(Engineer item)
    {
        throw new NotImplementedException();
    }
}
