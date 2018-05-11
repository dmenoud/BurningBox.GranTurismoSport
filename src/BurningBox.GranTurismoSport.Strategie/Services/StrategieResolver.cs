using System;
using System.Collections.Generic;
using System.Linq;
using BurningBox.GranTurismoSport.Strategie.BusinessModels;
using BurningBox.GranTurismoSport.Strategie.BusinessModels.Contracts;
using BurningBox.GranTurismoSport.Strategie.Services.Contracts;

namespace BurningBox.GranTurismoSport.Strategie.Services
{
    public class StrategieResolver : IStrategieResolver
    {
        public IStrategieResult Resolve(IRaceDefinition raceDefinition)
        {
            var tireTypes = raceDefinition.TireDefinitions.Select(t => t.TireType).ToList();
            var numberOfTiresType = tireTypes.Distinct().Count();
            if (numberOfTiresType != raceDefinition.TireDefinitions.Count)
            {
                throw new InvalidOperationException("TireDefinions is invalid, TireType must be distinct");
            }

            var strategieResults = new List<StrategieResult>();
            var fuelConsumptionPerLap = 100 / raceDefinition.NumberOfLapsWithFullFuel;

            foreach (var tireType in tireTypes)
            {
                foreach (PitStrategie pitStrategie in Enum.GetValues(typeof(PitStrategie)))
                {
                    var strategie = new StrategieResult
                                    {
                                        StartTireType = tireType,
                                        PitStops = new List<IPitStop>(),
                                        RaceTime = TimeSpan.Zero
                                    };
                    strategieResults.Add(strategie);

                    var tireDefinition = raceDefinition.TireDefinitions.First(t => t.TireType == tireType);

                    RunLaps(raceDefinition, tireDefinition, fuelConsumptionPerLap, pitStrategie, strategie);
                }

                // run
            }

            StrategieResult result = null;
            switch (raceDefinition.RaceMode)
            {
                case RaceMode.Race:
                    result = strategieResults.OrderBy(r => r.RaceTime).First();
                    break;
                case RaceMode.Endurance:
                    result = strategieResults.OrderBy(r => r.PitStops.Count).First();
                    break;
            }


            
            return result;
        }

        private void RunLaps(IRaceDefinition raceDefinition, ITireDefinition tireDefinition, double fuelConsumptionPerLap, PitStrategie pitStrategie, StrategieResult strategie)
        {
            var currentFuelRate = 100.0;
            var currentTiresRate = 100.0;
            var currentLapNumber = 0;
            var tireConsumptionPerLap = 100.0 / tireDefinition.OptimalNumberOfLaps;
            var needRefuel = false;
            var needChangeTires = false;

            do
            {
                currentLapNumber++;
                currentFuelRate -= fuelConsumptionPerLap;
                currentTiresRate -= tireConsumptionPerLap;

                if (currentFuelRate <= 20)
                {
                    needRefuel = true;
                }

                if (currentTiresRate <= 20)
                {
                    needChangeTires = true;
                }

                if (needChangeTires || needRefuel)
                {
                    switch(pitStrategie)
                    {
                        case PitStrategie.OnlyNeeded:
                            break;
                        case PitStrategie.FuelEachTime:
                            needRefuel = true;
                            break;
                        case PitStrategie.TiresEachTime:
                            needChangeTires = true;
                            break;
                        case PitStrategie.FuelAndTiresEachTime:
                            needRefuel = true;
                            needChangeTires = true;
                            break;
                    }


                    strategie.PitStops.Add(new PitStop
                                           {
                                               Refuel = needRefuel,
                                               ChangeTires = needChangeTires,
                                               LapsNumber = currentLapNumber,
                                               TireType = tireDefinition.TireType,
                                           });

                    if (needChangeTires)
                    {
                        strategie.RaceTime = strategie.RaceTime.Add(raceDefinition.TiresChangeDuration);
                        needChangeTires = false;
                        currentTiresRate = 100.0;
                    }

                    if (needRefuel)
                    {
                        strategie.RaceTime = strategie.RaceTime.Add(GetFuelFillingTime(raceDefinition, currentFuelRate));
                        needRefuel = false;
                        currentFuelRate = 100;
                    }

                    strategie.RaceTime = strategie.RaceTime.Add(raceDefinition.TimeLostForPitStop);
                }


                strategie.RaceTime = strategie.RaceTime.Add(tireDefinition.AverageLapTime);
            } while (!IsDone(raceDefinition, strategie, currentLapNumber));
        }

        private bool IsDone(IRaceDefinition raceDefinition, StrategieResult strategie, int currentLapNumber)
        {
            var done = false;
            switch(raceDefinition.RaceMode)
            {
                case RaceMode.Race:
                    if (currentLapNumber == raceDefinition.NumberOfLaps)
                    {
                        done = true;
                    }

                    break;
                case RaceMode.Endurance:
                    if (strategie.RaceTime >= raceDefinition.RaceDuration)
                    {
                        done = true;
                    }

                    break;
            }

            return done;
        }

        private TimeSpan GetFuelFillingTime(IRaceDefinition raceDefinition, double currentFuelRate)
        {
            // [s/%]
            var fillingSpeed = raceDefinition.FuelFillingDuration.TotalSeconds / raceDefinition.FuelToFillInPercent;

            return TimeSpan.FromSeconds((100 - currentFuelRate) * fillingSpeed);
        }
    }
}