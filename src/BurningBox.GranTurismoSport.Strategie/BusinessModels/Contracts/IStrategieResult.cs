using System;
using System.Collections.Generic;

namespace BurningBox.GranTurismoSport.Strategie.BusinessModels.Contracts
{
    public interface IStrategieResult
    {
        List<IPitStop> PitStops { get; set; }
        TireType StartTireType { get; set; }
        TimeSpan RaceTime { get; set; }
    }
}