
using DalApi;

namespace Dal;

sealed public class DalXml : IDal
{
    public IDependency Dependency => new DependencyImplementation() ;

    public ITask Task => new TaskImplementation();

    public IEngineer Engineer => new EngineerImplementation();
}
