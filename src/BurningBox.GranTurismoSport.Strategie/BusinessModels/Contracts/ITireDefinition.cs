using System;

namespace BurningBox.GranTurismoSport.Strategie.BusinessModels.Contracts
{
    public interface ITireDefinition
    {
        TireType TireType { get; }
        int OptimalNumberOfLaps { get; }
        TimeSpan AverageLapTime { get; }
    }
}