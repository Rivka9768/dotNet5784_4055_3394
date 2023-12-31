

using BlApi;
using BO;
using System.Diagnostics.Contracts;

namespace BlImplementation;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public void Create(BO.Task task)
    {
        if (task.Id <= 0)
            //לשאול את המורה לגבי ה כינוי של ה task
            throw new Exception();

        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public BO.Task? Read(int id)
    {
        DO.Task? task = _dal.Task.Read(id);
        if (task == null)
            throw new Exception();
        Status status = Status.Unscheduled;
        if (task.StartDate != null)
        {
            //לשאול את המורה מזה אומר בסיכון
            if (task.FinalDate > task.Deadline || DateTime.Now > task.Deadline)
                status = Status.InJeopardy;
            else
            {
                if (task.StartDate > DateTime.Now)
                    status = Status.Scheduled;
                else
                    status = Status.OnTrack;
            }
        }
        MilestoneInTask milestoneInTask = 



    }

    public IEnumerable<BO.Task?> ReadAll(Func<BO.Task, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(BO.Task task)
    {
        throw new NotImplementedException();
    }
}
