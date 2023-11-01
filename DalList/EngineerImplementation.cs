

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
            throw new NotImplementedException("can not create existing engineer");
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Engineer? Read(int id)
    {
        throw new NotImplementedException();
    }

    public List<Engineer> ReadAll()
    {
        throw new NotImplementedException();
    }

    public void Update(Engineer item)
    {
        throw new NotImplementedException();
    }
}
