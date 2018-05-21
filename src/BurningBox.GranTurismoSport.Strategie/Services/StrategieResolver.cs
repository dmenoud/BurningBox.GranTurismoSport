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
            var tiresProviders = GetProviders(5, raceDefinition.TiresDefinitions.Select(t => t.TiresType).OrderBy(t => t).ToList());
            var fuelProviders = GetProviders(4, new[]
                                             {
                                                 50,
                                                 60,
                                                 70,
                                                 80,
                                                 90,
                                                 100
                                             });

            foreach (PitStrategie pitStrategie in Enum.GetValues(typeof(PitStrategie)))
            {
                foreach (var tiresProvider in tiresProviders)
                {
                    foreach (var refuelProvider in fuelProviders)
                    {
                        var startTiresType = tiresProvider.GetNext();

                        var strategie = new StrategieResult
                                        {
                                            StartTiresType = startTiresType,
                                            PitStops = new List<IPitStop>(),
                                            RaceTime = TimeSpan.Zero
                                        };
                        strategieResults.Add(strategie);

                        RunLaps(raceDefinition, tiresProvider, fuelConsumptionPerLap, pitStrategie, strategie, refuelProvider);
                    }
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
                             .ThenBy(s => s.FuelEndState)
                             .First();
                    break;
            }

            return result;
        }

        private void RunLaps(IRaceDefinition raceDefinition, ItemsProvider<TiresType> tiresProvider, double fuelConsumptionPerLap, PitStrategie pitStrategie, IStrategieResult strategie, ItemsProvider<int> fuelProvider)
        {
            var tireDefinition = raceDefinition.TiresDefinitions.First(t => t.TiresType == strategie.StartTiresType);

            var currentFuelRate = 100.0;
            var currentTiresRate = 100.0;
            var tireConsumptionPerLap = 100.0 / tireDefinition.OptimalNumberOfLaps;
            var needRefuel = false;
            var needChangeTires = false;

            var nextRefuel = fuelProvider.GetNext();

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
                            needRefuel = currentFuelRate< nextRefuel  ;
                            break;
                        case PitStrategie.TiresEachTime:
                            needChangeTires = true;
                            break;
                        case PitStrategie.FuelAndTiresEachTime:
                            needRefuel = currentFuelRate < nextRefuel;
                            needChangeTires = true;
                            break;
                    }

                    var pitStop = new PitStop
                                  {
                                      Refuel = needRefuel ? nextRefuel : 0,
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
                        strategie.RaceTime = strategie.RaceTime.Add(GetFuelFillingTime(raceDefinition, currentFuelRate, nextRefuel));
                        needRefuel = false;
                        currentFuelRate = nextRefuel;
                        nextRefuel = fuelProvider.GetNext();
                    }

                    strategie.RaceTime = strategie.RaceTime.Add(raceDefinition.TimeLostForPitStop);
                }

                strategie.RaceTime = strategie.RaceTime.Add(tireDefinition.AverageLapTime);
                strategie.TiresEndState = currentTiresRate;
                strategie.FuelEndState = currentFuelRate;

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

        private TimeSpan GetFuelFillingTime(IRaceDefinition raceDefinition, double currentFuelRate, int refuel)
        {
            // [s/%]
            var fillingSpeed = raceDefinition.FuelFillingDuration.TotalSeconds / raceDefinition.FuelToFillInPercent;

            return TimeSpan.FromSeconds((refuel - currentFuelRate) * fillingSpeed);
        }


        private List<ItemsProvider<TItem>> GetProviders<TItem>(int sequenceSize ,IEnumerable<TItem> initialItems)
        {
            var items = initialItems.ToArray();
            var itemDictionary = InitItemDictionary(items);

            var sequences = GetSequences(sequenceSize, items.Length);

            var result = new List<ItemsProvider<TItem>>();
            foreach (var sequence in sequences)
            {
                var tiresTypes = sequence.Select(t => itemDictionary[t]).ToList();
                result.Add(new ItemsProvider<TItem>(tiresTypes));
            }

            return result;
        }

        private static Dictionary<int, TItem> InitItemDictionary<TItem>(IEnumerable<TItem> initialTires)
        {
            var tiresTypes = new Dictionary<int, TItem>();

            var i = 0;
            foreach (var tiresType in initialTires)
            {
                tiresTypes.Add(i++, tiresType);
            }

            return tiresTypes;
        }

        private List<int[]> GetSequences(int sequenceSize, int numberOfElements)
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

            var sequenceNumber = (int)Math.Pow(numberOfElements, sequenceSize);
            do
            {
                Increments(indexValues, sequenceSize, numberOfElements - 1, sequences);
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