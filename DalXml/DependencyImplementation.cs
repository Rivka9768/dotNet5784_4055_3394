

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
        throw new NotImplementedException();
    }

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(Dependency item)
    {
        throw new NotImplementedException();
    }
}
