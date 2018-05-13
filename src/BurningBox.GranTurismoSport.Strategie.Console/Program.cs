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
                                      new TiresDefinition(TiresType.RacingSuperSoft, 5, new TimeSpan(0, 0, 2, 14, 500)),
                                      new TiresDefinition(TiresType.RacingSoft, 6, new TimeSpan(0, 0, 2, 15, 800)),
                                      new TiresDefinition(TiresType.RacingMedium, 9, new TimeSpan(0, 0, 2, 24, 500)),
                                      new TiresDefinition(TiresType.RacingHard, 11, new TimeSpan(0, 0, 2, 30, 300)),
                                  };

            var def = new RaceDefinition(TimeSpan.FromHours(1),
                                         tireDefinitions, 
                                         "Mon Track",
                                         new TimeSpan(0, 0, 0, 5, 700),
                                         19,
                                         new TimeSpan(0, 0, 0, 4, 100),
                                         new TimeSpan(0, 0, 0, 28, 500),
                                         16.5,
                                         6);

            var result = strategieResolver.Resolve(def);
            
            System.Console.WriteLine("Strategie:");
            System.Console.WriteLine($"Startup tires : {result.StartTiresType}");
            System.Console.WriteLine($"Number of laps : {result.NumberOfLaps}");
            System.Console.WriteLine($"Race time : {result.RaceTime}");
            System.Console.WriteLine($"Pit stops count {result.PitStops.Count}");

            var i = 1;
            foreach (var pitStop in result.PitStops)
            {
                System.Console.WriteLine($"\tNo {i++,3:N0}, Lap = {pitStop.LapNumber,3:N0}, Refuel = {pitStop.Refuel,-5}, ChangeTires = {pitStop.ChangeTires,-5}, Tires = {pitStop.TiresType,-16}");
            }


            System.Console.ReadLine();
        }
    }
}