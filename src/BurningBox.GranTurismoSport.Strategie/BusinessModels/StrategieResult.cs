using System;
using System.Collections.Generic;
using BurningBox.GranTurismoSport.Strategie.BusinessModels.Contracts;

namespace BurningBox.GranTurismoSport.Strategie.BusinessModels
{
    public class StrategieResult : IStrategieResult
    {
        public List<IPitStop> PitStops { get; set; }
        public TiresType StartTiresType { get; set; }
        public TimeSpan RaceTime { get; set; }
    }
}