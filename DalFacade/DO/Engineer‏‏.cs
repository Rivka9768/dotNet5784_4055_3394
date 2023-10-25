
namespace DO;
/// <summary>
/// Engineer Entity represents a engineer with all its properties.
/// </summary>
/// <param name="Id">the engineer's id</param>
/// <param name="Name">the engineer's name</param>
/// <param name="Email">the engineer's email address</param>
/// <param name="Level"> the engineer's experience level</param>
/// <param name="SaleryPerHour">the amount the engineer gets per hour</param>
public record Engineer‏‏
(
    int Id,
    string Name,
    string Email,
    int Level,
    double SaleryPerHour
);