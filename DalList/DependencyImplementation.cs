
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class DependencyImplementation : IDependency
{
    public int Create(Dependency item)
    {
        int id = DataSource.Config.NextDependencyId;
        DataSource.Dependencys.Add(item: item with { Id =id });
        return id;
    }

    public void Delete(int id)
    {
        Dependency dependency = (from d in DataSource.Dependencys
                          let dId = d.Id
                          where dId == id
                          select d).FirstOrDefault()!;
        if (dependency == null)
            throw new Exception("An object of type Dependency with such an ID does not exist");
        DataSource.Dependencys.Remove(dependency);
    }

    public Dependency? Read(int id)
    {

        Dependency dependency = (from d in DataSource.Dependencys
                          let dId = d.Id
                          where dId == id
                          select d).FirstOrDefault()!;
        return dependency;
    }

/*    public List<Dependency> ReadAll()
    {
        return new List<Dependency>(DataSource.Dependencys);
    }*/

    public void Update(Dependency item)
    {
        Dependency dependency = (from d in DataSource.Dependencys
                          let dId = d.Id
                          where dId == item.Id
                          select d).FirstOrDefault()!;
        if (dependency==null)
            throw new Exception("An object of type Dependency with such an ID does not exist");
        DataSource.Dependencys.Remove(dependency);
        DataSource.Dependencys.Add(item);
    }


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
