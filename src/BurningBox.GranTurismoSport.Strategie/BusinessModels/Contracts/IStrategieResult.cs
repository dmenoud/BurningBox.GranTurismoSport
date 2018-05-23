using System;
using System.Collections.Generic;

namespace BurningBox.GranTurismoSport.Strategie.BusinessModels.Contracts
{
    public interface IStrategieResult
    {
        List<IPitStop> PitStops { get; set; }
        TiresType StartTiresType { get; set; }
        TimeSpan RaceTime { get; set; }
        int NumberOfLaps { get; set; }
        double FuelEndState { get; set; }
        double TiresEndState { get; set; }
        int RaceDistance { get; set; }

        int DistanceAtEnduranceTime { get; set; }
    }
}