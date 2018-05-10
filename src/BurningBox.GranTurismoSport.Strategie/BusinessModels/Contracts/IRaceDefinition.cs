using System;
using System.Collections.Generic;

namespace BurningBox.GranTurismoSport.Strategie.BusinessModels.Contracts
{
    public interface IRaceDefinition
    {
        string TrackName { get; set; }
        RaceMode RaceMode { get; }
        int TireWearFactor { get; set; }
        int FuelConsumptionFactor { get; set; }
        TimeSpan RaceDuration { get; }
        int NumberOfLaps { get; }
        List<ITireDefinition> TireDefinition { get; }
    }
}