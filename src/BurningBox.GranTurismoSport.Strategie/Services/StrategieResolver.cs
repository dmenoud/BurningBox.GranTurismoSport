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
            var tireTypes = raceDefinition.TiresDefinitions.Select(t => t.TiresType).ToList();
            var numberOfTiresType = tireTypes.Distinct().Count();
            if (numberOfTiresType != raceDefinition.TiresDefinitions.Count)
            {
                throw new InvalidOperationException("TiresDefinions is invalid, TiresType must be distinct");
            }

            var strategieResults = new List<StrategieResult>();
            var fuelConsumptionPerLap = 100 / raceDefinition.NumberOfLapsWithFullFuel;
            var tiresProviders = GetTiresProviders(raceDefinition);

            foreach (PitStrategie pitStrategie in Enum.GetValues(typeof(PitStrategie)))
            {
                foreach (var tiresProvider in tiresProviders)
                {
                    var startTiresType = tiresProvider.GetNext();

                    var strategie = new StrategieResult
                                    {
                                        StartTiresType = startTiresType,
                                        PitStops = new List<IPitStop>(),
                                        RaceTime = TimeSpan.Zero
                                    };
                    strategieResults.Add(strategie);

                    RunLaps(raceDefinition, tiresProvider, fuelConsumptionPerLap, pitStrategie, strategie);
                }
            }
            
            StrategieResult result = null;
            switch(raceDefinition.RaceMode)
            {
                case RaceMode.Race:
                    result = strategieResults.OrderBy(s => s.RaceTime).First();
                    break;
                case RaceMode.Endurance:
                    result = strategieResults
                             .OrderByDescending(s => s.NumberOfLaps)
                             .ThenBy(s => s.RaceTime)
                             .First();
                    break;
            }

            return result;
        }

        private void RunLaps(IRaceDefinition raceDefinition, TiresProvider tiresProvider, double fuelConsumptionPerLap, PitStrategie pitStrategie, IStrategieResult strategie)
        {
            var tireDefinition = raceDefinition.TiresDefinitions.First(t => t.TiresType == strategie.StartTiresType);

            var currentFuelRate = 100.0;
            var currentTiresRate = 100.0;
            var tireConsumptionPerLap = 100.0 / tireDefinition.OptimalNumberOfLaps;
            var needRefuel = false;
            var needChangeTires = false;

            do
            {
                strategie.NumberOfLaps++;
                currentFuelRate -= fuelConsumptionPerLap;
                currentTiresRate -= tireConsumptionPerLap;

                if (currentFuelRate <= raceDefinition.FuelReservePercent)
                {
                    needRefuel = true;
                }

                if (currentTiresRate <= 10)
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

                    var pitStop = new PitStop
                                  {
                                      Refuel = needRefuel,
                                      ChangeTires = needChangeTires,
                                      LapNumber = strategie.NumberOfLaps,
                                      TiresType = tireDefinition.TiresType,
                                      FuelState = currentFuelRate,
                                      TiresState = currentTiresRate

                                  };
                    strategie.PitStops.Add(pitStop);

                    if (needChangeTires)
                    {
                        strategie.RaceTime = strategie.RaceTime.Add(raceDefinition.TiresChangeDuration);
                        needChangeTires = false;
                        currentTiresRate = 100.0;
                        var nextTires = tiresProvider.GetNext();
                        tireDefinition = raceDefinition.TiresDefinitions.First(t => t.TiresType == nextTires);
                        tireConsumptionPerLap = 100.0 / tireDefinition.OptimalNumberOfLaps;
                        pitStop.TiresType = tireDefinition.TiresType;
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
            } while (!IsDone(raceDefinition, strategie));
        }

        private bool IsDone(IRaceDefinition raceDefinition, IStrategieResult strategie)
        {
            var done = false;
            switch(raceDefinition.RaceMode)
            {
                case RaceMode.Race:
                    if (strategie.NumberOfLaps == raceDefinition.NumberOfLaps)
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


        private List<TiresProvider> GetTiresProviders(IRaceDefinition raceDefinition)
        {
            var initialTires = raceDefinition.TiresDefinitions.Select(t => t.TiresType).OrderBy(t => t).ToList();

            var tiresTypeDictonnary = InitTiresTypeDictonnary(initialTires);

            var sequences = GetSequences(5, initialTires);


            var result = new List<TiresProvider>();
            foreach (var sequence in sequences)
            {
                var tiresTypes = sequence.Select(t => tiresTypeDictonnary[t]).ToList();
                result.Add(new TiresProvider(tiresTypes));
            }

            return result;
        }

        private static Dictionary<int, TiresType> InitTiresTypeDictonnary(List<TiresType> initialTires)
        {
            var tiresTypes = new Dictionary<int, TiresType>();

            var i = 0;
            foreach (var tiresType in initialTires)
            {
                tiresTypes.Add(i++, tiresType);
            }

            return tiresTypes;
        }

        private List<int[]> GetSequences(int sequenceSize, List<TiresType> initialTires)
        {
            var sequences = new List<int[]>
                            {
                                new int[sequenceSize]
                            };

            var indexValues = new Dictionary<int, int>();
            for (var j = 0; j < sequenceSize; j++)
            {
                indexValues.Add(j, 0);
            }

            var sequenceNumber = (int)Math.Pow(initialTires.Count, sequenceSize);
            do
            {
                Increments(indexValues, sequenceSize, initialTires.Count - 1, sequences);
            } while (sequences.Count < sequenceNumber);

            return sequences;
        }

        private void Increments(Dictionary<int, int> indexValues, int sequenceSize, int maxValue, List<int[]> sequences)
        {
            indexValues[0]++;
            if (indexValues[0] > maxValue)
            {
                indexValues[0] = 0;

                for (var i = 1; i < sequenceSize; i++)
                {
                    indexValues[i]++;
                    if (indexValues[i] > maxValue)
                    {
                        indexValues[i] = 0;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            var seq = new int[sequenceSize];
            sequences.Add(seq);
            for (var i = 0; i < sequenceSize; i++)
            {
                seq[i] = indexValues[i];
            }
        }
    }
}