

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
        throw new DalAlreadyExistsException($"Engineer with ID={item.Id} already exists");

    }

    public void Delete(int id)
    {
        Engineer engineer = (from e in DataSource.Engineers
                        let eId = e.Id
                        where eId == id
                        select e).FirstOrDefault()!;
        if (engineer == null) 
            throw new DalDeletionImpossible($"Can't delete Engineer with ID={id}");
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


    public void Update(Engineer item)
    {
        Engineer engineer = (from e in DataSource.Engineers
                        let eId = e.Id
                        where eId == item.Id
                        select e).FirstOrDefault()!;
        if (engineer==null)
            throw new DalDoesNotExistException($"Engineer with ID={item.Id} does not exists");
        DataSource.Engineers.Remove(engineer);
        DataSource.Engineers.Add(item);
    }

    public Engineer? Read(Func<Engineer, bool> filter)
    {
            return (from item in DataSource.Engineers
                    where filter(item)
                    select item).FirstOrDefault();
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
