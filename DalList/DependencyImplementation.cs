
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class DependencyImplementation : IDependency
{
    /// <summary>
    /// creates a new dependency
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int Create(Dependency item)
    {
        int id = DataSource.Config.NextDependencyId;
        DataSource.Dependencys.Add(item: item with { Id =id });
        return id;
    }

    /// <summary>
    /// delete a dependency
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id)
    {
        Dependency dependency = (from d in DataSource.Dependencys
                          let dId = d.Id
                          where dId == id
                          select d).FirstOrDefault()!;
        if (dependency == null)
            throw new DalDoesNotExistException($"Dependency with ID={id} does not exists");
        DataSource.Dependencys.Remove(dependency);
    }

    /// <summary>
    /// find a dependency by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Dependency? Read(int id)
    {

        Dependency dependency = (from d in DataSource.Dependencys
                          let dId = d.Id
                          where dId == id
                          select d).FirstOrDefault()!;
        return dependency;
    }

    /// <summary>
    /// updates a specific dependency
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Update(Dependency item)
    {
        Dependency dependency = (from d in DataSource.Dependencys
                          let dId = d.Id
                          where dId == item.Id
                          select d).FirstOrDefault()!;
        if (dependency==null)
            throw new DalDoesNotExistException($"Dependency with ID={item.Id} does not exists");
        DataSource.Dependencys.Remove(dependency);
        DataSource.Dependencys.Add(item);
    }

    /// <summary>
    /// finds a dependency by a boolian condition
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public Dependency? Read(Func<Dependency, bool> filter)
    {
            return (from item in DataSource.Dependencys
                    where filter(item)
                    select item).FirstOrDefault();
    }

    /// <summary>
    /// returns all dependencies which answers to the boolian condition or if the function is called with no parameters than returns all dependencies
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<Dependency> ReadAll(Func<Dependency, bool>? filter = null)
        {
            if (filter != null)
            {
                return from item in DataSource.Dependencys
                       where filter(item)
                       select item;
            }
            return from item in DataSource.Dependencys
                   select item;
        }

}
