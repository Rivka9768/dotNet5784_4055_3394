
using DalApi;
using System.Diagnostics;

namespace Dal;

sealed internal class DalXml : IDal
{
    public static IDal Instance { get; } = new Lazy<DalXml>(true).Value;
    private DalXml() { }

    public IDependency Dependency => new DependencyImplementation() ;

    public ITask Task => new TaskImplementation();

    public IEngineer Engineer => new EngineerImplementation();
}
