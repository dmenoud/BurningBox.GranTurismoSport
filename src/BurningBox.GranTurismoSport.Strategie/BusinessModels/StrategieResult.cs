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
        public int NumberOfLaps { get; set; }
        public double FuelEndState { get; set; }
        public double TiresEndState { get; set; }
        public int RaceDistance { get; set; }
        public int DistanceAtEnduranceTime { get; set; }
    }
}