using System;
namespace DO;
/// <summary>
/// Task entity which repesents a task and its properties.
/// </summary>
/// <param name="Id">the task's id</param>
/// <param name="Description">the description of the task</param>
/// <param name="TaskNickname">the task's nickname</param>
/// <param name="Milestone">???????????????????????????????</param>
/// <param name="ProductionDate">the date the tassk was created</param>
/// <param name="StartDate">the start date of the task</param>
/// <param name="EstimatedDate">estimated date to complete the task</param>
/// <param name="FinalDate">final date of completing the task</param>
/// <param name="Deadline">the deadline for completing the task</param>
/// <param name="ProductDescription">the description of the product</param>
/// <param name="Remarks">remarks on the task</param>
/// <param name="EngineerId">the id of the engineer dealing with the task</param>
/// <param name="Difficulty">the task's level of difficulty</param>
public record Task
(
int Id,
string Description,
string TaskNickname,
bool Milestone,
DateTime ProductionDate,
DateTime StartDate,
DateTime EstimatedDate,
DateTime FinalDate,
DateTime Deadline,
string ProductDescription,
string Remarks,
int EngineerId,
EngineerExperience Difficulty
);




 

