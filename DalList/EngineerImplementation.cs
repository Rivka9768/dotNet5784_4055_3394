

namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class EngineerImplementation : IEngineer
{
    public int Create(Engineer item)
    {
        Engineer? engineer = DataSource.Engineers.Find((e)=>e.Id == item.Id);
        if (engineer == null)
        {
            DataSource.Engineers.Add(item);
            return item.Id;
        }
            throw new NotImplementedException("An object of type Engineer with such an ID already exists");
    }

    public void Delete(int id)
    {
        Engineer? engineer = DataSource.Engineers.Find((e) => e.Id == id);
        if (engineer == null) 
            throw new NotImplementedException("An object of type Engineer with such an ID does not exist");
        DataSource.Engineers.Remove(engineer);
    }

    public Engineer? Read(int id)
    {
        Engineer? engineer = DataSource.Engineers.Find((e) => e.Id == id);
        return engineer;
    }

    public List<Engineer> ReadAll()
    {
        return new List<Engineer>(DataSource.Engineers);
    }

    public void Update(Engineer item)
    {
        Engineer? engineer = DataSource.Engineers.Find((e) => e.Id == item.Id);
        if(engineer==null)
            throw new NotImplementedException("An object of type Engineer with such an ID does not exist");
        DataSource.Engineers.Remove(engineer);
        DataSource.Engineers.Add(item);
    }
}
