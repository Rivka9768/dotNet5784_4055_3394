﻿

using BlApi;

namespace BlImplementation;

internal class Bl : IBl
{
    public IEngineer Engineer =>  new EngineerImplementation();

    public ITask Task =>  new TaskImplementation();

    public IMilestone milestone =>  new MilestonImplementation();
}
