using System;
using System.Collections.Generic;
using BurningBox.GranTurismoSport.Strategie.BusinessModels;
using BurningBox.GranTurismoSport.Strategie.BusinessModels.Contracts;
using BurningBox.GranTurismoSport.Strategie.Services;

namespace BurningBox.GranTurismoSport.Strategie.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var strategieResolver = new StrategieResolver();


            var tireDefinitions = new List<ITiresDefinition>
                                  {
                                      new TiresDefinition(TiresType.RacingSuperSoft, 5, new TimeSpan(0, 0, 2, 13, 900)),
                                      new TiresDefinition(TiresType.RacingSoft, 6, new TimeSpan(0, 0, 2, 14, 900)),
                                      new TiresDefinition(TiresType.RacingMedium, 9, new TimeSpan(0, 0, 2, 16, 200)),
                                      //new TiresDefinition(TiresType.RacingHard, 12, new TimeSpan(0, 0, 2, 18, 300)),
                                  };

            var def = new RaceDefinition(TimeSpan.FromHours(1),
                                         tireDefinitions,
                                         "Mon Track",
                                         new TimeSpan(0, 0, 0, 9, 500),
                                         60,
                                         new TimeSpan(0, 0, 0, 4, 100),
                                         new TimeSpan(0, 0, 0, 10, 0),
                                         16.5,
                                         10,
                                         6213);


            System.Console.WriteLine("Parmeters:");
            System.Console.WriteLine("-----------------------------------------------");
            System.Console.WriteLine("Tires definitions:");
            foreach (var tiresDefinition in tireDefinitions)
            {
                System.Console.WriteLine($"\t{tiresDefinition}");
            }

            System.Console.WriteLine("Race definition:");
            System.Console.WriteLine($"\t{nameof(def.TrackName)} = {def.TrackName}");
            System.Console.WriteLine($"\t{nameof(def.TiresChangeDuration)} = {def.TiresChangeDuration}");
            System.Console.WriteLine($"\t{nameof(def.FuelToFillInPercent)} = {def.FuelToFillInPercent}");
            System.Console.WriteLine($"\t{nameof(def.FuelFillingDuration)} = {def.FuelFillingDuration}");
            System.Console.WriteLine($"\t{nameof(def.TimeLostForPitStop)} = {def.TimeLostForPitStop}");
            System.Console.WriteLine($"\t{nameof(def.NumberOfLapsWithFullFuel)} = {def.NumberOfLapsWithFullFuel}");
            System.Console.WriteLine($"\t{nameof(def.FuelReservePercent)} = {def.FuelReservePercent}");
            System.Console.WriteLine();

            var result = strategieResolver.Resolve(def);

            System.Console.WriteLine();
            System.Console.WriteLine("Result:");
            System.Console.WriteLine("-----------------------------------------------");
            System.Console.WriteLine($"Startup tires : {result.StartTiresType}");
            System.Console.WriteLine($"Number of laps : {result.NumberOfLaps}");
            System.Console.WriteLine($"Race time : {result.RaceTime}");
            System.Console.WriteLine($"Pit stops count {result.PitStops.Count} :");
            var i = 1;
            foreach (var pitStop in result.PitStops)
            {
                System.Console.WriteLine($"\tNo {i++,3:N0}, Lap = {pitStop.LapNumber,3:N0}, Refuel = {pitStop.Refuel,3:F0}%, ChangeTires = {pitStop.ChangeTires,-5}, Tires = {pitStop.TiresType,-16}, FuelState = {pitStop.FuelState,6:F2}%, TiresState = {pitStop.TiresState,6:F2}%");
            }

            System.Console.WriteLine($"Fuel end state {result.FuelEndState,3:N3}%");
            System.Console.WriteLine($"Tires end state {result.TiresEndState,3:N3}%");

            System.Console.ReadLine();
        }
    }
}