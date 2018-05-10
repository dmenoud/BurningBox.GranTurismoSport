using System;
using System.Collections.Generic;
using BurningBox.GranTurismoSport.Strategie.BusinessModels.Contracts;

namespace BurningBox.GranTurismoSport.Strategie.BusinessModels
{
    public class RaceDefinition : IRaceDefinition
    {
        public RaceDefinition(TimeSpan raceDuration, List<ITireDefinition> tireDefinition)
        {
            this.RaceDuration = raceDuration;
            this.TireDefinition = tireDefinition;
            this.RaceMode = RaceMode.Endurance;
        }

        public RaceDefinition(int numberOfLaps, List<ITireDefinition> tireDefinition)
        {
            this.NumberOfLaps = numberOfLaps;
            this.TireDefinition = tireDefinition;
            this.RaceMode = RaceMode.Race;
        }

        public string TrackName { get; set; }
        public RaceMode RaceMode { get; }
        public int TireWearFactor { get; set; }
        public int FuelConsumptionFactor { get; set; }
        public TimeSpan RaceDuration { get; }
        public int NumberOfLaps { get; }
        public List<ITireDefinition> TireDefinition { get; }
    }
}