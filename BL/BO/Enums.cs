

namespace BO;

public enum EngineerExperience
{
    Novice,
    AdvancedBeginner,
    Competent,
    Proficient,
    Expert
}

public enum Status
{
    Unscheduled,//doesn't yet have a start date
    Scheduled,//has start date but not yet completed
    OnTrack,//in progress after start date
    InJeopardy//completed after the deadline
}
