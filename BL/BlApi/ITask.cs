
namespace BlApi;

public interface ITask
{

    public IEnumerable<BO.TaskInList?> ReadAll(Func<BO.Task, bool>? filter = null);
    public BO.Task? Read(int id);
    public void Create(BO.Task task);
    public void Update(BO.Task task);
    public void Delete(int id);
}
