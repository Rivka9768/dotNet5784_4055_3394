
namespace DO;

/// <summary>
/// Dependency entity repesents an entity that specifies the dependencies between tasks and its properties.
/// </summary>
/// <param name="Id">the private id of the dependency</param>
/// <param name="IdPreviousTask">the id of the previous task </param>
/// <param name="IdDependantTask">the id of the dependant task</param>
public record Dependency
(
  int Id,
  int IdPreviousTask,
  int IdDependantTask
)
{
	public Dependency() : this(0,0,0) { }

}