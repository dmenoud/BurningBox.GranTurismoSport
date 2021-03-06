﻿using System;
using BurningBox.GranTurismoSport.Strategie.BusinessModels.Contracts;

namespace BurningBox.GranTurismoSport.Strategie.BusinessModels
{
    public class TiresDefinition : ITiresDefinition
    {
        public TiresDefinition(TiresType tiresType, int optimalNumberOfLaps, TimeSpan averageLapTime)
        {
            this.TiresType = tiresType;
            this.OptimalNumberOfLaps = optimalNumberOfLaps;
            this.AverageLapTime = averageLapTime;
        }

        public TiresType TiresType { get; }
        public int OptimalNumberOfLaps { get; }
        public TimeSpan AverageLapTime { get; }

        public override string ToString()
        {
            return $"Tires: {this.TiresType,-16}, Optimal number of laps: {this.OptimalNumberOfLaps,3:N0}, Average lap time: {this.AverageLapTime}";
        }
    }
}