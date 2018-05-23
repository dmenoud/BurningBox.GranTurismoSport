using System;
using System.Collections.Generic;
using BurningBox.GranTurismoSport.Strategie.BusinessModels.Contracts;
using JetBrains.Annotations;

namespace BurningBox.GranTurismoSport.Strategie.BusinessModels
{
    public class RaceDefinition : IRaceDefinition
    {
        public RaceDefinition(TimeSpan raceDuration, [NotNull] List<ITiresDefinition> tireDefinitions, string trackName, TimeSpan fuelFillingDuration, int fuelToFillInPercent, TimeSpan tiresChangeDuration, TimeSpan timeLostForPitStop, double numberOfLapsWithFullFuel, double fuelReservePercent, int circuitLenght)
        {
            this.RaceDuration = raceDuration;
            this.TiresDefinitions = tireDefinitions;
            this.TrackName = trackName;
            this.FuelFillingDuration = fuelFillingDuration;
            this.FuelToFillInPercent = fuelToFillInPercent;
            this.TiresChangeDuration = tiresChangeDuration;
            this.TimeLostForPitStop = timeLostForPitStop;
            this.NumberOfLapsWithFullFuel = numberOfLapsWithFullFuel;
            this.FuelReservePercent = fuelReservePercent;
            this.CircuitLenght = circuitLenght;
            this.RaceMode = RaceMode.Endurance;
        }

        public RaceDefinition(int numberOfLaps, [NotNull] List<ITiresDefinition> tireDefinitions, string trackName, TimeSpan fuelFillingDuration, int fuelToFillInPercent, TimeSpan tiresChangeDuration, TimeSpan timeLostForPitStop, double numberOfLapsWithFullFuel, double fuelReservePercent, int circuitLenght)
        {
            this.NumberOfLaps = numberOfLaps;
            this.TiresDefinitions = tireDefinitions;
            this.TrackName = trackName;
            this.FuelFillingDuration = fuelFillingDuration;
            this.FuelToFillInPercent = fuelToFillInPercent;
            this.TiresChangeDuration = tiresChangeDuration;
            this.TimeLostForPitStop = timeLostForPitStop;
            this.NumberOfLapsWithFullFuel = numberOfLapsWithFullFuel;
            this.FuelReservePercent = fuelReservePercent;
            this.CircuitLenght = circuitLenght;
            this.RaceMode = RaceMode.Race;
        }

        public string TrackName { get;}
        public RaceMode RaceMode { get; }
        public TimeSpan RaceDuration { get; }
        public int NumberOfLaps { get; }
        public List<ITiresDefinition> TiresDefinitions { get; }
        public TimeSpan FuelFillingDuration { get; }
        public int FuelToFillInPercent { get; }
        public TimeSpan TiresChangeDuration { get; }
        public TimeSpan TimeLostForPitStop { get; }
        public double NumberOfLapsWithFullFuel { get; }
        public double FuelReservePercent { get; }
        public int CircuitLenght { get; }
    }
}