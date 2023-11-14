

namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class EngineerImplementation : IEngineer
{
    public int Create(Engineer item)
    {

        Engineer engineer = (from e in DataSource.Engineers
                        let eId = e.Id
                          where eId == item.Id
                          select e).FirstOrDefault()!;
        if (engineer == null)
        {
            DataSource.Engineers.Add(item);
            return item.Id;
        }
            throw new Exception("An object of type Engineer with such an ID already exists");
    }

    public void Delete(int id)
    {
        Engineer engineer = (from e in DataSource.Engineers
                        let eId = e.Id
                        where eId == id
                        select e).FirstOrDefault()!;
        if (engineer == null) 
            throw new Exception("An object of type Engineer with such an ID does not exist");
        DataSource.Engineers.Remove(engineer);
    }

    public Engineer? Read(int id)
    {
        Engineer engineer = (from e in DataSource.Engineers
                        let eId = e.Id
                        where eId == id
                        select e).FirstOrDefault()!;
        return engineer;
    }

/*    public List<Engineer> ReadAll()
    {
        return new List<Engineer>(DataSource.Engineers);
    }*/

    public void Update(Engineer item)
    {
        Engineer engineer = (from e in DataSource.Engineers
                        let eId = e.Id
                        where eId == item.Id
                        select e).FirstOrDefault()!;
        if (engineer==null)
            throw new Exception("An object of type Engineer with such an ID does not exist");
        DataSource.Engineers.Remove(engineer);
        DataSource.Engineers.Add(item);
    }
    public IEnumerable<Engineer> ReadAll(Func<Engineer, bool>? filter = null)
    {
        if (filter != null)
        {
            return from item in DataSource.Engineers
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Engineers
               select item;
    }
}
