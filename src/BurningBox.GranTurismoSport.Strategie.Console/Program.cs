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
                                      new TiresDefinition(TiresType.RacingMedium, 8, new TimeSpan(0, 0, 2, 17, 500)),
                                      new TiresDefinition(TiresType.RacingHard, 10, new TimeSpan(0, 0, 2, 22, 300)),
                                  };

            var def = new RaceDefinition(TimeSpan.FromHours(1),
                                         tireDefinitions, 
                                         "Mon Track",
                                         new TimeSpan(0, 0, 0, 5, 700),
                                         19,
                                         new TimeSpan(0, 0, 0, 4, 100),
                                         new TimeSpan(0, 0, 0, 28, 500),
                                         16.5);


            var result = strategieResolver.Resolve(def);

            

        }
    }
}