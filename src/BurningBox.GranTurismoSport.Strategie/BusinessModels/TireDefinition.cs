using System;
using BurningBox.GranTurismoSport.Strategie.BusinessModels.Contracts;

namespace BurningBox.GranTurismoSport.Strategie.BusinessModels
{
    public class TireDefinition : ITireDefinition
    {
        public TireDefinition(TireType tireType, int optimalNumberOfLaps, TimeSpan averageLapTime)
        {
            this.TireType = tireType;
            this.OptimalNumberOfLaps = optimalNumberOfLaps;
            this.AverageLapTime = averageLapTime;
        }

        public TireType TireType { get; }
        public int OptimalNumberOfLaps { get; }
        public TimeSpan AverageLapTime { get; }
    }
}