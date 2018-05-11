using System;
using System.Collections.Generic;
using BurningBox.GranTurismoSport.Strategie.BusinessModels.Contracts;
using JetBrains.Annotations;

namespace BurningBox.GranTurismoSport.Strategie.BusinessModels
{
    public class RaceDefinition : IRaceDefinition
    {
        public RaceDefinition(TimeSpan raceDuration, [NotNull] List<ITiresDefinition> tireDefinition, string trackName, TimeSpan fuelFillingDuration, int fuelToFillInPercent, TimeSpan tiresChangeDuration, TimeSpan timeLostForPitStop, double numberOfLapsWithFullFuel)
        {
            this.RaceDuration = raceDuration;
            this.TiresDefinitions = tireDefinition;
            this.TrackName = trackName;
            this.FuelFillingDuration = fuelFillingDuration;
            this.FuelToFillInPercent = fuelToFillInPercent;
            this.TiresChangeDuration = tiresChangeDuration;
            this.TimeLostForPitStop = timeLostForPitStop;
            this.NumberOfLapsWithFullFuel = numberOfLapsWithFullFuel;
            this.RaceMode = RaceMode.Endurance;
        }

        public RaceDefinition(int numberOfLaps, [NotNull] List<ITiresDefinition> tireDefinition, string trackName, TimeSpan fuelFillingDuration, int fuelToFillInPercent, TimeSpan tiresChangeDuration, TimeSpan timeLostForPitStop, double numberOfLapsWithFullFuel)
        {
            this.NumberOfLaps = numberOfLaps;
            this.TiresDefinitions = tireDefinition;
            this.TrackName = trackName;
            this.FuelFillingDuration = fuelFillingDuration;
            this.FuelToFillInPercent = fuelToFillInPercent;
            this.TiresChangeDuration = tiresChangeDuration;
            this.TimeLostForPitStop = timeLostForPitStop;
            this.NumberOfLapsWithFullFuel = numberOfLapsWithFullFuel;
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
    }
}