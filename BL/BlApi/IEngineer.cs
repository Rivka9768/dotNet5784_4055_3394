
namespace BlApi;

public interface IEngineer
{
    public IEnumerable<BO.EngineerInList> ReadAll(Func<BO.Engineer, bool>? filter = null);
    public BO.Engineer? Read(int id);
    public void Create(BO.Engineer engineer);
    public void Delete(int id);
    public void Update(BO.Engineer engineer);
}
