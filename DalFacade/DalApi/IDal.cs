namespace DalApi;

public interface IDal
{
    IDependency Dependency { get; }
    ITask Task { get; }
    IEngineer Engineer { get; } 
}
