

using DalApi;
using DO;

namespace Dal;

internal class DependencyImplementation : IDependency
{
    public int Create(Dependency item)
    {
        int id = XMLTools.GetAndIncreaseNextId("Config", "NextDependencyId");
       List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>("dependencys");
        dependencies.Add(item);
        XMLTools.SaveListToXMLSerializer<Dependency>(dependencies, "dependencys");
        return id;
    }

    public void Delete(int id)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>("dependencys");
        Dependency dependency = (from d in dependencies
                                 let dId = d.Id
                                 where dId == id
                                 select d).FirstOrDefault()!;
        if (dependency == null)
            throw new DalDoesNotExistException($"Dependency with ID={id} does not exists");
        dependencies.Remove(dependency);
        XMLTools.SaveListToXMLSerializer<Dependency>(dependencies, "dependencys");
    }

    public Dependency? Read(int id)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>("dependencys");
        Dependency dependency = (from d in dependencies
                                 let dId = d.Id
                                 where dId == id
                                 select d).FirstOrDefault()!;
        return dependency;
    }

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>("dependencys");
        return (from item in dependencies
                where filter(item)
                select item).FirstOrDefault();
    }

    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>("dependencys");
        if (filter != null)
        {
            return from item in dependencies
                   where filter(item)
                   select item;
        }
        return from item in dependencies
               select item;
    }

    public void Update(Dependency item)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>("dependencys");
        Dependency dependency = (from d in dependencies
                                 let dId = d.Id
                                 where dId == item.Id
                                 select d).FirstOrDefault()!;
        if (dependency == null)
            throw new DalDoesNotExistException($"Dependency with ID={item.Id} does not exists");
        dependencies.Remove(dependency);
        dependencies.Add(item);
        XMLTools.SaveListToXMLSerializer<Dependency>(dependencies, "dependencys");
    }
}
