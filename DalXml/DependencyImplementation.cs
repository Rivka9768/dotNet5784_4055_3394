

using DalApi;
using DO;

namespace Dal;

internal class DependencyImplementation : IDependency
{
    /// <summary>
    /// creates a new dependency
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int Create(Dependency item)
    {
        int id = Config.NextDependencyId;
       List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>("dependencys");
        dependencies.Add(item: item with { Id = id });
        XMLTools.SaveListToXMLSerializer<Dependency>(dependencies, "dependencys");
        return id;
    }

    /// <summary>
    /// delete a dependency
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
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

    /// <summary>
    /// find a dependency by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Dependency? Read(int id)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>("dependencys");
        Dependency dependency = (from d in dependencies
                                 let dId = d.Id
                                 where dId == id
                                 select d).FirstOrDefault()!;
        return dependency;
    }

    /// <summary>
    /// finds a dependency by a boolian condition
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public Dependency? Read(Func<Dependency, bool> filter)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>("dependencys");
        return (from item in dependencies
                where filter(item)
                select item).FirstOrDefault();
    }

    /// <summary>
    /// returns all dependencies which answers to the boolian condition or if the function is called with no parameters than returns all dependencies
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
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
/// <summary>
    /// updates a specific dependency
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
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
