using System;
using System.Collections.Generic;

namespace BurningBox.GranTurismoSport.Strategie.BusinessModels.Contracts
{
    public interface IStrategieResult
    {
        List<IPitStop> PitStops { get; set; }
        TiresType StartTiresType { get; set; }
        TimeSpan RaceTime { get; set; }
    }
}