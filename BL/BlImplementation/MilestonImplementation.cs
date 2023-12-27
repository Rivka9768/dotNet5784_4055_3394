

using BlApi;
using BO;

namespace BlImplementation;

internal class MilestonImplementation : IMilestone
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public Milestone? Read(int id)
    {
        throw new NotImplementedException();
    }

    public Milestone Update(Milestone milestone)
    {
        throw new NotImplementedException();
    }
}
