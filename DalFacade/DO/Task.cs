using System;
namespace DO;
/// <summary>
/// Task entity which repesents a task and its properties.
/// </summary>
/// <param name="Id">the task's id</param>
/// <param name="Description">the description of the task</param>
/// <param name="TaskNickname">the task's nickname</param>
/// <param name="Milestone">boolian flag that shows if the task has a milestone</param>
/// <param name="ProductionDate">the date the task was created</param>
/// <param name="StartDate">the start date of the task</param>
/// <param name="EstimatedEndDate">estimated date to complete the task</param>
/// <param name="FinalDate">final date of completing the task</param>
/// <param name="Deadline">the deadline for completing the task</param>
/// <param name="Products">the products of the task</param>
/// <param name="Remarks">remarks on the task</param>
/// <param name="EngineerId">the id of the engineer dealing with the task</param>
/// <param name="Difficulty">the task's level of difficulty</param>
public record Task
(
int Id ,
string Description,
DateTime ProductionDate,
DateTime Deadline,
EngineerExperience Difficulty,
int? EngineerId = null,
bool ?Milestone = false,
DateTime? StartDate=null,
DateTime? EstimatedEndDate=null,
DateTime? FinalDate = null,
string? TaskNickname = "",
string? Remarks = "",
string? Products = ""
);




 

