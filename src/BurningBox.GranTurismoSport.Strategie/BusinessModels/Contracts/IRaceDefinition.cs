﻿using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace BurningBox.GranTurismoSport.Strategie.BusinessModels.Contracts
{
    public interface IRaceDefinition
    {
        string TrackName { get; }
        RaceMode RaceMode { get; }
        TimeSpan RaceDuration { get; }
        int NumberOfLaps { get; }
        List<ITiresDefinition> TiresDefinitions { get; }
        TimeSpan FuelFillingDuration { get; }
        int FuelToFillInPercent { get; }
        TimeSpan TiresChangeDuration { get; }
        TimeSpan TimeLostForPitStop { get; }
        double NumberOfLapsWithFullFuel { get; }
        double FuelReservePercent { get; }
    }
}   