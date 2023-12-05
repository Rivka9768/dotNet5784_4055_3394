

namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class EngineerImplementation : IEngineer
{
    /// <summary>
    /// creates a new engineer
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
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

    /// <summary>
    /// delete a engineer
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id)
    {
        Engineer engineer = (from e in DataSource.Engineers
                        let eId = e.Id
                        where eId == id
                        select e).FirstOrDefault()!;
        if (engineer == null) 
            throw new DalDoesNotExistException($"Engineer with ID={id} does not exists");
        DataSource.Engineers.Remove(engineer);
    }
    /// <summary>
    /// find a engineer by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Engineer? Read(int id)
    {
        Engineer engineer = (from e in DataSource.Engineers
                        let eId = e.Id
                        where eId == id
                        select e).FirstOrDefault()!;
        return engineer;
    }


    /// <summary>
    /// updates a specific engineer
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
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

    /// <summary>
    /// finds a engineer by a boolian condition
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public Engineer? Read(Func<Engineer, bool> filter)
    {
            return (from item in DataSource.Engineers
                    where filter(item)
                    select item).FirstOrDefault();
    }
    /// <summary>
    /// returns all engineers which answers to the boolian condition or if the function is called with no parameters than returns all engineers
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
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
