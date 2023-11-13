
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class DependencyImplementation : IDependency
{
    public int Create(Dependency item)
    {
        int id = DataSource.Config.NextDependencyId;
        DataSource.Dependencys.Add(item: item with { Id =id });
        return id;
    }

    public void Delete(int id)
    {
        Dependency? dependency = DataSource.Dependencys.Find((d) => d.Id == id);
        if (dependency == null)
            throw new Exception("An object of type Dependency with such an ID does not exist");
        DataSource.Dependencys.Remove(dependency);
    }

    public Dependency? Read(int id)
    {
        Dependency? dependency = DataSource.Dependencys.Find((d) => d.Id == id);
        return dependency;
    }

    public List<Dependency> ReadAll()
    {
        return new List<Dependency>(DataSource.Dependencys);
    }

    public void Update(Dependency item)
    {
        Dependency? dependency = DataSource.Dependencys.Find((d) => d.Id == item.Id);
        if(dependency==null)
            throw new Exception("An object of type Dependency with such an ID does not exist");
        DataSource.Dependencys.Remove(dependency);
        DataSource.Dependencys.Add(item);
    }
}
