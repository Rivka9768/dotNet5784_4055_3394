namespace Dal;

using DalApi;

sealed internal class DalList : IDal
{
    public static IDal Instance { get; } = new Lazy<DalList>(true).Value;
    private DalList() { }

    public IDependency Dependency => new DependencyImplementation();

    public ITask Task => new TaskImplementation();

    public IEngineer Engineer => new EngineerImplementation();
  

}
