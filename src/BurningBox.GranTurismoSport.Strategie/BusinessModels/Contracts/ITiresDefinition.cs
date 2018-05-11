using System;

namespace BurningBox.GranTurismoSport.Strategie.BusinessModels.Contracts
{
    public interface ITiresDefinition
    {
        TiresType TiresType { get; }
        int OptimalNumberOfLaps { get; }
        TimeSpan AverageLapTime { get; }
    }
}